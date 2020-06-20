using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// カメラの向きから相対的に移動する
/// </summary>
public class PlayerMovementBasedCamera : MonoBehaviour
{
    [SerializeField] private float runHorizontalSpeed = 3f;
    [SerializeField] private float jumpVerticalSpeed = 3f;
    [SerializeField] private float gravityVerticalForce = 3f;
    [SerializeField] private float spinJumpVerticalSpeed = 3f;
    [SerializeField] private float wallKickHorizontalSpeed = 3f;
    [SerializeField] private float wallKickVerticalSpeed = 3f;
    [SerializeField] private float backFlipHorizontalSpeed = 3f;
    [SerializeField] private float backFlipVerticalSpeed = 3f;
    [SerializeField] private float longJumpHorizontalSpeed = 3f;
    [SerializeField] private float longJumpVerticalSpeed = 3f;
    [SerializeField] private float normalAirHorizontalForce = 3f;
    [SerializeField] private float spinJumpAirHorizontalForce = 3f;
    [SerializeField] private float hipDropVerticalSpeed = 3f;

    [SerializeField] private float maxNormalAirHorizontalSpeed = 3f;
    [SerializeField] private float maxNormalAirVerticalSpeed = 3f;
    [SerializeField] private float maxSpinJumpAirHorizontalSpeed = 2f;
    [SerializeField] private float maxSpinJumpAirVerticalSpeed = 2f;
    [SerializeField] private float maxStickingWallFallSpeed = 1f;


    private Rigidbody _rigidbody;
    private bool _isGrounded = false;
    private E_State currentState = E_State.Standing;
    private E_ActionFlag waitingAction = E_ActionFlag.None;

    private CharacterController _characterController;
    private PlayerAnimation _playerAnimation;

    private Vector3 addForceDownPower = Vector3.down;
    private Vector3 _velocity;

    private MonobitEngine.MonobitView _monobitView;

    [field: SerializeField]
    [field: RenameField("centerPosition")]
    public Transform CenterPosition { get; private set; }

    /// <summary>
    /// x: Horizontal, y: Vertical
    /// </summary>
    private Vector2 inputVelocity = Vector2.zero;


    /// <summary>
    /// 壁に空中で張り付いているか(stateのひとつとして持ってもいい)
    /// </summary>
    public bool IsStickingWall { get; set; } = false;

    public bool IsGrounded => this._isGrounded;

    /// <summary>
    /// 張り付いている壁の法線ベクトル
    /// </summary>
    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;

    public PlayerAnimation _PlayerAnimation => this._playerAnimation;

    public Vector3 Velocity { get; set; }


    private void Awake()
    {
        this._monobitView = GetComponent<MonobitEngine.MonobitView>();

        if (!this._monobitView.isMine) return;
        this._rigidbody = GetComponent<Rigidbody>();
        this._playerAnimation = GetComponent<PlayerAnimation>();
        this._characterController = GetComponent<CharacterController>();

        this._characterController
            .ObserveEveryValueChanged(x => x.isGrounded)
            .ThrottleFrame(5)
            .Subscribe(x => this._isGrounded = x);
    }

    private void Update()
    {
        if (!this._monobitView.isMine) return;

        FirstUpdateInit();

        UpdateInput();
        UpdateAction();
        UpdateMovement();
        UpdateState();
        UpdateAnimation();
        UpdateFixRotation();
    }

    private void DebugFunc()
    {
        //this.transform.RotateAround(this.centerPosition, this.transform.right, 100f * Time.deltaTime);
    }

    /// <summary>
    /// 毎フレームUpdateで最初に呼ばれる初期化処理
    /// </summary>
    private void FirstUpdateInit()
    {
        this.waitingAction = E_ActionFlag.None;
    }
    /// <summary>
    /// プレイヤーの入力を受け取る
    /// </summary>
    private void UpdateInput()
    {
        this.inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (this.currentState == E_State.Standing && this.inputVelocity != Vector2.zero) this.currentState = E_State.Running;
        switch (this.currentState)
        {
            case E_State.Standing:
            case E_State.Running:
                if (Input.GetButtonDown("Jump")) this.waitingAction = E_ActionFlag.NormalJump;
                else if (Input.GetButtonDown("SpinJump")) this.waitingAction = E_ActionFlag.SpinJump;
                else if (Input.GetButtonDown("LongJump")) this.waitingAction = E_ActionFlag.LongJump;
                else if (Input.GetButtonDown("BackFlip")) this.waitingAction = E_ActionFlag.BackFlip;
                break;
            case E_State.StickingWall:
                if (Input.GetButtonDown("Jump")) this.waitingAction = E_ActionFlag.WallKick;
                else if (Input.GetButtonDown("HipDrop")) this.waitingAction = E_ActionFlag.HipDrop;
                break;
            case E_State.JumpToTop:
            case E_State.TopOfJump:
            case E_State.Falling:
            case E_State.LongJumpToTop:
            case E_State.SpinJumping:
            case E_State.BackFliping:
                if (Input.GetButtonDown("HipDrop")) this.waitingAction = E_ActionFlag.HipDrop;
                break;
        }
    }

    private void UpdateAction()
    {
        switch (this.waitingAction)
        {
            case E_ActionFlag.NormalJump:
                NormalJump();
                break;
            case E_ActionFlag.LongJump:
                LongJump();
                break;
            case E_ActionFlag.HipDrop:
                HipDrop();
                break;
            case E_ActionFlag.WallKick:
                WallKick();
                break;
            case E_ActionFlag.SpinJump:
                SpinJump();
                break;
            case E_ActionFlag.BackFlip:
                BackFlip();
                break;
        }
    }

    /// <summary>
    /// currentStateで分岐された、移動入力処理
    /// </summary>
    private void UpdateMovement()
    {
        if (this._isGrounded) this._velocity = Vector3.zero;
        if(this.currentState != E_State.HipDropping) this._velocity.y -= this.gravityVerticalForce * Time.deltaTime;

        switch (this.currentState)
        {
            case E_State.Standing:
            case E_State.Running:
                MoveOnGround();
                break;
            case E_State.StickingWall:
                MoveStickWall();
                break;
            case E_State.SpinJumping:
                MoveSpinAir();
                break;
            case E_State.JumpToTop: 
            case E_State.TopOfJump:
            case E_State.Falling:
            case E_State.LongJumpToTop:
                MoveNormalAir();
                break;
        }

        //velocityに応じた移動処理
        this._characterController.Move(this._velocity * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤーの移動速度に応じて状態を変える(ジャンプなどの入力は別)
    /// </summary>
    private void UpdateState()
    {

        //ここで接地判定をおこなうため、jumpなどの処理時に念のため_isGrounded = falseにするべき
        if (this._isGrounded)
        {
            if (this.inputVelocity == Vector2.zero) this.currentState = E_State.Standing;
            else this.currentState = E_State.Running;
        }
        else
        {
            if (this.currentState == E_State.HipDropping) return; //ヒップドロップ中は着地するまで遷移しない
            if (this.currentState == E_State.JumpToTop && this._velocity.y > 0f) this.currentState = E_State.TopOfJump;
            else if (this._velocity.y <= -0f && (this.currentState != E_State.SpinJumping && this.currentState != E_State.StickingWall && this.currentState != E_State.BackFliping)) this.currentState = E_State.Falling;
        }
    }

    private void UpdateAnimation()
    {
        switch (this.currentState)
        {
            case E_State.Standing:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.Standing);
                break;
            case E_State.Running:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.Running);
                break;
            case E_State.TopOfJump:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopOfJump);
                break;
            case E_State.Falling:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopToGround);
                break;
            case E_State.StickingWall:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.StickingWall);
                break;


            case E_State.Salute:
                break;
            case E_State.KnellDown:
                break;
        }

        this._playerAnimation.PlayerAnimator.SetBool("IsLanding", this._isGrounded);
    }

    /// <summary>
    /// 現在の状態に合わせて、プレイヤーのrotation.xzを修正する(万が一ずれたとき用)
    /// </summary>
    private void UpdateFixRotation()
    {
        if(this.currentState != E_State.BackFliping && this.currentState != E_State.HipDropping)
        {
            this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);
        }
    }

    /// <summary>
    /// 地上での入力による走行移動
    /// </summary>
    private void MoveOnGround()
    {
        //カメラの角度に合わせて入力方向に移動
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * this.inputVelocity.y + Camera.main.transform.right * this.inputVelocity.x;
        //this._velocity = moveForward * this.runHorizontalSpeed * Time.deltaTime + new Vector3(0f, this._velocity.y, 0f); 
        this._velocity = moveForward * this.runHorizontalSpeed + this.addForceDownPower;

        //this._velocity += this.addForceDownPower; //いらんかも

        //移動方向に回転
        if (moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveForward), 0.2f);
        }
    }

    /// <summary>
    /// 地上からの通常ジャンプ
    /// </summary>
    private void NormalJump()
    {
        this._velocity.y = this.jumpVerticalSpeed;
        this._isGrounded = false;

        //進行方向を向く
        if(!(this._velocity.x == 0f && this._velocity.z == 0f)) this.transform.forward = new Vector3(this._velocity.x, 0f, this._velocity.z);

        this.currentState = E_State.JumpToTop;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
    }

    /// <summary>
    /// 通常状態の空中でのvelocity制御
    /// </summary>
    private void MoveNormalAir()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * this.inputVelocity.y + Camera.main.transform.right * this.inputVelocity.x;
        Vector3 moveVelocity = moveForward * this.normalAirHorizontalForce * Time.deltaTime + this._velocity;

        //最高速度を超えていなければ入力した横方向に加速
        if (Mathf.Sqrt(moveVelocity.x * moveVelocity.x + moveVelocity.z * moveVelocity.z) < this.maxNormalAirHorizontalSpeed) this._velocity = new Vector3(moveVelocity.x, this._velocity.y, moveVelocity.z);

        //vertical下方向に最高速度を越えていれば、最高速度にする
        if (this._velocity.y < -1 * this.maxNormalAirVerticalSpeed) this._velocity.y = -1 * this.maxNormalAirVerticalSpeed;
    }

    /// <summary>
    /// 壁張り付き状態でのvelocity制御
    /// </summary>
    private void MoveStickWall()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * this.inputVelocity.y + Camera.main.transform.right * this.inputVelocity.x;
        Vector3 moveVelocity = moveForward * this.normalAirHorizontalForce * Time.deltaTime + new Vector3(this._velocity.x, 0f, this._velocity.z);


        //壁方向への速度は0にする(いつでもすぐに壁から離れられるようにするため)
        float incidenceAngle = Vector3.SignedAngle(moveVelocity, -1 * this.NormalOfStickingWall, Vector3.up); //移動ベクトルと壁の法線ベクトル間の角度
        if(incidenceAngle > -90 && incidenceAngle < 90) //壁方向への速度があるとき→壁方向の速さのみ0にする
        {
            //壁に平行方向への速さに(ベクトルはそのまま)
            moveVelocity *= Mathf.Abs(Mathf.Sin(incidenceAngle * Mathf.Deg2Rad));

            //速度ベクトルを壁に平行方向に回転
            if (incidenceAngle <= 0) moveVelocity = Quaternion.Euler(0f, 90 + incidenceAngle, 0f) * moveVelocity; //正面右方向
            else if (incidenceAngle > 0) moveVelocity = Quaternion.Euler(0f, -1 * (90 - incidenceAngle), 0f) * moveVelocity; //正面左方向
        }

        //最高速度を超えていなければ入力した水平方向に加速
        if (Mathf.Sqrt(moveVelocity.x * moveVelocity.x + moveVelocity.z * moveVelocity.z) < this.maxNormalAirHorizontalSpeed) this._velocity = new Vector3(moveVelocity.x, this._velocity.y, moveVelocity.z);

        if (this._velocity.y < -1 * this.maxStickingWallFallSpeed) this._velocity.y = -1 * this.maxStickingWallFallSpeed;
    }

    /// <summary>
    /// スピン状態の空中でのvelocity制御。加速度運動ではなく、等速運動でもいいかも？
    /// </summary>
    private void MoveSpinAir()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * this.inputVelocity.y + Camera.main.transform.right * this.inputVelocity.x;
        Vector3 moveVelocity = moveForward * this.spinJumpAirHorizontalForce * Time.deltaTime + this._velocity;

        //最高速度を超えていなければ入力した横方向に加速
        if (Mathf.Sqrt(moveVelocity.x * moveVelocity.x + moveVelocity.z * moveVelocity.z) < this.maxSpinJumpAirHorizontalSpeed) this._velocity = new Vector3(moveVelocity.x, this._velocity.y, moveVelocity.z);

        //vertical下方向に最高速度を越えていれば、最高速度にする
        if (this._velocity.y < -1 * this.maxSpinJumpAirVerticalSpeed) this._velocity.y = -1 * this.maxSpinJumpAirVerticalSpeed;

        //回転運動
        this.transform.Rotate(new Vector3(0f, 2000f, 0f) * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// バク転
    /// </summary>
    private void BackFlip()
    {
        //this._velocity.y = this.backFlipVerticalSpeed;
        this._isGrounded = false;

        //進行方向を向く
        if (!(this._velocity.x == 0f && this._velocity.z == 0f)) this.transform.forward = new Vector3(this._velocity.x, 0f, this._velocity.z);
        this._velocity = this.transform.forward * -1 * this.backFlipHorizontalSpeed + this.transform.up * this.backFlipVerticalSpeed;

        StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this.transform, this.CenterPosition, E_TransformAxis.Right, -360f, 1f));

        this.currentState = E_State.BackFliping;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.BackFlip);
    }

    /// <summary>
    /// 幅跳び
    /// </summary>
    private void LongJump()
    {
        this._isGrounded = false;
        //とりあえず地上の移動速度の0.3倍を初速に足しておく
        this._velocity = this.transform.forward * this.longJumpHorizontalSpeed + Vector3.up * this.longJumpVerticalSpeed + this._velocity * 0.3f;

        this.transform.forward = new Vector3(this._velocity.x, 0f, this._velocity.z);

        //とりあえず通常ジャンプと同じ
        this.currentState = E_State.JumpToTop;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
    }

    /// <summary>
    /// スピンジャンプ。水平方向に移動しやすい。
    /// </summary>
    private void SpinJump()
    {
        this._isGrounded = false;
        this._velocity.y = this.spinJumpVerticalSpeed;

        this.currentState = E_State.SpinJumping;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.SpinJump);
    }

    /// <summary>
    /// ヒップドロップ。空中でvelocity0に。一瞬待って真下に等速運動
    /// </summary>
    private void HipDrop()
    {
        StopAllCoroutines(); //現在はbackFlipのコルーチンのみ
        this._velocity = Vector3.zero;
        this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);
        this.currentState = E_State.HipDropping;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.HipDrop);
        StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this.transform, this.CenterPosition, E_TransformAxis.Right, 360f, 0.14f));
        StartCoroutine(CoroutineManager.DelayMethod(0.15f, () =>
        {
            this._velocity.y = -1 * this.hipDropVerticalSpeed;
        }));
    }

    /// <summary>
    /// 壁キック
    /// </summary>
    public void WallKick()
    {
        if (this._velocity.y >= -0.05f) return;
        Debug.Log("壁キック");

        this._isGrounded = false;
        this.currentState = E_State.JumpToTop;

        //法線ベクトルのyの大きさに制限を加える？
        this.transform.forward = this.NormalOfStickingWall;

        //移動させる
        this._velocity = new Vector3(this.NormalOfStickingWall.x * this.wallKickHorizontalSpeed + this._velocity.x, this.wallKickVerticalSpeed, this.NormalOfStickingWall.z * this.wallKickHorizontalSpeed + this._velocity.z);

        //animation
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
    }

    /// <summary>
    /// 壁に張り付く。張り付いたときに壁方向を向き、水平方向の速度を0にする
    /// </summary>
    /// <param name="normalOfStickingWall">壁面の垂直方向(自身のほうへの)</param>
    public void StickWall(Vector3 normalOfStickingWall)
    {
        if (this._isGrounded) return;

        this._velocity = new Vector3(0f, this._velocity.y, 0f);
        this.transform.rotation = Quaternion.LookRotation(-1 * normalOfStickingWall, Vector3.up);
        this.NormalOfStickingWall = normalOfStickingWall;
        this.currentState = E_State.StickingWall;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.StickingWall);
    }

    /// <summary>
    /// 壁張り付き状態から、落下状態へ移行
    /// </summary>
    public void StopStickingWall()
    {
        if (this._isGrounded) return;

        this.currentState = E_State.Falling;
        this.NormalOfStickingWall = Vector3.zero;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopToGround);
    }

    /// <summary>
    /// 現在の状態。移動処理の分岐やanimation再生に利用
    /// </summary>
    public enum E_State
    {
        Standing,
        Running,
        JumpToTop,
        TopOfJump, //animation流すだけなのでいらないかも
        Falling,
        SpinJumping,
        StickingWall,
        LongJumpToTop, //animationは普通のジャンプと変わらない(初速度だけ違う)→いらないかも
        HipDropping,
        BackFliping,


        Damaged,
        Salute,
        KnellDown,
        Other
    }

    /// <summary>
    /// 瞬間的入力でおこなわれるプレイヤーのアクション
    /// </summary>
    public enum E_ActionFlag
    {
        None,
        NormalJump,
        LongJump,
        SpinJump,
        WallKick,
        HipDrop,
        BackFlip,
    }

}

public enum E_Vector
{
    X,
    Y,
    Z,
    XY,
    XZ,
    YZ,
    XYZ,
}
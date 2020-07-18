using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using KanKikuchi.AudioManager;
using System.Linq;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float normalAirHorizontalForce = 3f;
    [SerializeField] private float spinJumpAirHorizontalForce = 3f;
    [SerializeField] private float hipDropVerticalSpeed = 3f;

    [SerializeField] private float maxNormalAirHorizontalSpeed = 3f;
    [SerializeField] private float maxNormalAirVerticalSpeed = 3f;
    [SerializeField] private float maxSpinJumpAirHorizontalSpeed = 2f;
    [SerializeField] private float maxSpinJumpAirVerticalSpeed = 2f;
    [SerializeField] private float maxStickingWallFallSpeed = 1f;
    [SerializeField] private float maxStickingWallHorizontalSpeed = 5f;

    [SerializeField] private float rateOfRunHorizontalSpeedOnAccelerationGround = 3f;
    [SerializeField] private float jumpSecondVerticalSpeed = 8f;
    [SerializeField] private float jumpThirdVerticalSpeed = 5f;
    [SerializeField] private GameObject stickingWallSmoke;
    [SerializeField] private GameObject hipDropOnGroundShock;
    [SerializeField] private GameObject runningSmoke;

    private Rigidbody _rigidbody;
    private bool _isGrounded = false;
    private E_State currentState = E_State.Standing;
    private E_ActionFlag waitingAction = E_ActionFlag.None;
    private Transform _hitHeadCheck;
    private Transform _groundCheck;
    private WallKickTrigger _wallKickTrigger;

    private CharacterController _characterController;
    private PlayerAnimation _playerAnimation;

    private Vector3 addForceDownPower = Vector3.down;
    private Vector3 _velocity;
    
    private Vector3 addVelocityThisFrame = Vector3.zero;
    private Vector3 addVelocityThisFrameInGrounded = Vector3.zero;
    private Vector3 addPositionThisFrame = Vector3.zero;
    private Vector3 addPositionThisFrameInGrounded = Vector3.zero;

    private MonobitEngine.MonobitView _monobitView;

    private int pressJumpButtonFrame = 0;
    private bool checkPressJumpButton = false;
    private bool isTitleScene = false;

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

    public bool IsOnAccelerationGround { get; set; } = false;

    public E_State CurrentState => this.currentState;

    /// <summary>
    /// 張り付いている壁の法線ベクトル
    /// </summary>
    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;

    public PlayerAnimation _PlayerAnimation => this._playerAnimation;

    public bool AbleBreakByHipDrop { get; set; } = true;
    private void Awake()
    {
        this._monobitView = GetComponent<MonobitEngine.MonobitView>();

        if (this._monobitView != null && !this._monobitView.isMine) return;
        this._rigidbody = GetComponent<Rigidbody>();
        this._playerAnimation = GetComponent<PlayerAnimation>();
        this._characterController = GetComponent<CharacterController>();
        this._hitHeadCheck = this.transform.Find("HitHeadCheck").transform;
        this._groundCheck = this.transform.Find("GroundCheck").transform;
        this._wallKickTrigger = this.transform.Find("WallCheck").GetComponent<WallKickTrigger>();

        this._characterController
            .ObserveEveryValueChanged(x => x.isGrounded)
            .ThrottleFrame(5)
            .Subscribe(x => this._isGrounded = x);
        this.isTitleScene = SceneManager.GetActiveScene().name == "Title";
    }

    private void Update()
    {
        if (this._monobitView != null && !this._monobitView.isMine || !StageTimeManager.Instance.IsPlayerMoving) return;

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
        if (!this.isTitleScene) this.inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else this.inputVelocity = new Vector2(Input.GetAxis("Horizontal"), 0f);

        if (this.inputVelocity.magnitude > 1f) this.inputVelocity = this.inputVelocity.normalized;

        if (this.currentState == E_State.Standing && this.inputVelocity != Vector2.zero) this.currentState = E_State.Running;
        switch (this.currentState)
        {
            case E_State.Standing:
            case E_State.Running:
                if (Input.GetButtonDown("Jump")) this.waitingAction = E_ActionFlag.NormalJump;
                else if (Input.GetButtonDown("SpinJump")) this.waitingAction = E_ActionFlag.SpinJump;
                else if (Input.GetButtonDown("BackFlip")) this.waitingAction = E_ActionFlag.BackFlip;
                break;
            case E_State.StickingWall:
                if (Input.GetButtonDown("Jump")) this.waitingAction = E_ActionFlag.WallKick;
                else if (Input.GetButtonDown("HipDrop")) this.waitingAction = E_ActionFlag.HipDrop;
                break;
            case E_State.JumpToTop:
            case E_State.TopOfJump:
                if (Input.GetButtonDown("HipDrop"))
                {
                    this.waitingAction = E_ActionFlag.HipDrop;
                    this.checkPressJumpButton = false;
                    this.pressJumpButtonFrame = 0;
                }
                else if (this.checkPressJumpButton)
                {
                    if (Input.GetButton("Jump"))
                    {
                        this.pressJumpButtonFrame++;
                        if (this.pressJumpButtonFrame == 4) this._velocity.y += this.jumpSecondVerticalSpeed;
                        if (this.pressJumpButtonFrame == 7) this._velocity.y += this.jumpThirdVerticalSpeed;
                    }
                    else
                    {
                        this.checkPressJumpButton = false;
                        this.pressJumpButtonFrame = 0;
                    }
                }
                break;
            case E_State.SpinJumping:
                if (Input.GetButtonDown("HipDrop")) this.waitingAction = E_ActionFlag.HipDrop;
                else if (this.checkPressJumpButton)
                {
                    if (Input.GetButton("SpinJump"))
                    {
                        this.pressJumpButtonFrame++;
                        if (this.pressJumpButtonFrame == 4) this._velocity.y += this.jumpSecondVerticalSpeed;
                        if (this.pressJumpButtonFrame == 7) this._velocity.y += this.jumpThirdVerticalSpeed;
                    }
                    else
                    {
                        this.checkPressJumpButton = false;
                        this.pressJumpButtonFrame = 0;
                    }
                }
                break;
            case E_State.Falling:
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
                MoveNormalAir();
                break;
        }

        //velocityに応じた移動処理
        this._velocity += this.addVelocityThisFrame;
        if (this._isGrounded) this._velocity += this.addVelocityThisFrameInGrounded;
        this._characterController.Move(this._velocity * Time.deltaTime + this.addPositionThisFrame + this.addPositionThisFrameInGrounded);
        this.addVelocityThisFrame = Vector3.zero;
        this.addVelocityThisFrameInGrounded = Vector3.zero;
        this.addPositionThisFrame = Vector3.zero;
        this.addPositionThisFrameInGrounded = Vector3.zero;
    }

    /// <summary>
    /// プレイヤーの移動速度に応じて状態を変える(ジャンプなどの入力は別)
    /// </summary>
    private void UpdateState()
    {

        //ここで接地判定をおこなうため、jumpなどの処理時に念のため_isGrounded = falseにするべき
        if (this._isGrounded)
        {
            if (this.currentState == E_State.BackFliping)
            {
                this.StopAllCoroutineOfRotation();
            }
            if (this.currentState == E_State.HipDropping)
            {
                this.AbleBreakByHipDrop = true;
                SEManager.Instance.Play(SEPath.HIP_DROP_GROUNDED, volumeRate: 0.5f);
                SEManager.Instance.Play(SEPath.HIP_DROP_GROUNDED1, volumeRate: 0.5f);
                Instantiate(this.hipDropOnGroundShock, this.transform.position, Quaternion.identity);
            }
            if(this.currentState == E_State.StickingWall) SEManager.Instance.Stop(SEPath.STICKING_WALL);

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

        if (this.IsOnAccelerationGround)
        {
            this._velocity *= this.rateOfRunHorizontalSpeedOnAccelerationGround;
            if (this.currentState == E_State.Running && !SEManager.Instance.GetCurrentAudioNames().Any(s => s == "Acceleration"))
            {
                SEManager.Instance.Play(SEPath.ACCELERATION, volumeRate: 0.3f);
            }
        }

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

        //進行方向を向く→入力方向と速度が一致しないこともあるため撤廃
        //if(!(this._velocity.x == 0f && this._velocity.z == 0f)) this.transform.forward = new Vector3(this._velocity.x, 0f, this._velocity.z);

        this.currentState = E_State.JumpToTop;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
        SEManager.Instance.Play(SEPath.JUMP_VOICE0);
        SEManager.Instance.Play(SEPath.JUMP_WIND0, volumeRate: 0.5f);
        this.checkPressJumpButton = true;
        StartCoroutine(CoroutineManager.DelayMethod(8, () =>
        {
            this.checkPressJumpButton = false;
            this.pressJumpButtonFrame = 0;
        }));
    }

    /// <summary>
    /// 上への速度を追加、ジャンプモーションへ移行
    /// </summary>
    /// <param name="jumpVerticalSpeed">上方向への初速度</param>
    public void Jump(float jumpVerticalSpeed)
    {
        this._velocity.y = 0f;
        if (this._velocity.magnitude >= this.maxNormalAirHorizontalSpeed) this._velocity *= this.maxNormalAirHorizontalSpeed / this._velocity.magnitude;
        this._velocity.y = jumpVerticalSpeed;
        this._isGrounded = false;

        if (this.currentState != E_State.SpinJumping)
        {
            this.currentState = E_State.JumpToTop;
            this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
        }
        SEManager.Instance.Play(SEPath.JUMP_VOICE0);
        SEManager.Instance.Play(SEPath.JUMP_WIND0, volumeRate: 0.5f);
        SEManager.Instance.Play(SEPath.TRAMPOLINE_JUMP, volumeRate: 0.5f);

        this.checkPressJumpButton = true;
        StartCoroutine(CoroutineManager.DelayMethod(8, () =>
        {
            this.checkPressJumpButton = false;
            this.pressJumpButtonFrame = 0;
        }));
        this._wallKickTrigger.ResetTrigger();
    }

    /// <summary>
    /// 通常状態の空中でのvelocity制御
    /// </summary>
    private void MoveNormalAir()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * this.inputVelocity.y + Camera.main.transform.right * this.inputVelocity.x;
        Vector3 moveVelocity = moveForward * this.normalAirHorizontalForce * Time.deltaTime + this._velocity;

        float horizontalSpeed = Mathf.Sqrt(moveVelocity.x * moveVelocity.x + moveVelocity.z * moveVelocity.z);
        if (horizontalSpeed > this.maxNormalAirHorizontalSpeed && horizontalSpeed < this.maxNormalAirHorizontalSpeed + 3f)
        {
            moveVelocity.x *= this.maxNormalAirHorizontalSpeed / horizontalSpeed;
            moveVelocity.z *= this.maxNormalAirHorizontalSpeed / horizontalSpeed;
        } else if (horizontalSpeed > this.maxNormalAirHorizontalSpeed * this.rateOfRunHorizontalSpeedOnAccelerationGround)
        {
            moveVelocity.x *= (this.maxNormalAirHorizontalSpeed * this.rateOfRunHorizontalSpeedOnAccelerationGround) / horizontalSpeed;
            moveVelocity.z *= (this.maxNormalAirHorizontalSpeed * this.rateOfRunHorizontalSpeedOnAccelerationGround) / horizontalSpeed;
        }

        this._velocity = new Vector3(moveVelocity.x, this._velocity.y, moveVelocity.z);

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
        if (incidenceAngle > -90 && incidenceAngle < 90) //壁方向への速度があるとき→壁方向の速さのみ0にする
        {
            //壁に平行方向への速さに(ベクトルはそのまま)
            moveVelocity *= Mathf.Abs(Mathf.Sin(incidenceAngle * Mathf.Deg2Rad));

            //速度ベクトルを壁に平行方向に回転
            if (incidenceAngle <= 0) moveVelocity = Quaternion.Euler(0f, 90 + incidenceAngle, 0f) * moveVelocity; //正面右方向
            else if (incidenceAngle > 0) moveVelocity = Quaternion.Euler(0f, -1 * (90 - incidenceAngle), 0f) * moveVelocity; //正面左方向
        }

        float horizontalSpeed = Mathf.Sqrt(moveVelocity.x * moveVelocity.x + moveVelocity.z * moveVelocity.z);
        if (horizontalSpeed > this.maxNormalAirHorizontalSpeed)
        {
            moveVelocity.x *= this.maxNormalAirHorizontalSpeed / horizontalSpeed;
            moveVelocity.z *= this.maxNormalAirHorizontalSpeed / horizontalSpeed;
        }
        this._velocity = new Vector3(moveVelocity.x, this._velocity.y, moveVelocity.z);

        if (this._velocity.y < -1 * this.maxStickingWallFallSpeed) this._velocity.y = -1 * this.maxStickingWallFallSpeed;

        Instantiate(this.stickingWallSmoke, this.transform.position + this.transform.forward * 0.08f, Quaternion.identity);

        if (!SEManager.Instance.GetCurrentAudioNames().Any(s => s == "StickingWall")) SEManager.Instance.Play(SEPath.STICKING_WALL, volumeRate: 0.5f);
    }

    /// <summary>
    /// スピン状態の空中でのvelocity制御。加速度運動ではなく、等速運動でもいいかも？
    /// </summary>
    private void MoveSpinAir()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * this.inputVelocity.y + Camera.main.transform.right * this.inputVelocity.x;
        Vector3 moveVelocity = moveForward * this.spinJumpAirHorizontalForce * Time.deltaTime + this._velocity;

        float horizontalSpeed = Mathf.Sqrt(moveVelocity.x * moveVelocity.x + moveVelocity.z * moveVelocity.z);
        if (horizontalSpeed > this.maxSpinJumpAirHorizontalSpeed && horizontalSpeed < this.maxSpinJumpAirHorizontalSpeed + 3f)
        {
            moveVelocity.x *= this.maxSpinJumpAirHorizontalSpeed / horizontalSpeed;
            moveVelocity.z *= this.maxSpinJumpAirHorizontalSpeed / horizontalSpeed;
        }else if(horizontalSpeed > this.maxSpinJumpAirHorizontalSpeed * this.rateOfRunHorizontalSpeedOnAccelerationGround)
        {
            moveVelocity.x *= (this.maxSpinJumpAirHorizontalSpeed * this.rateOfRunHorizontalSpeedOnAccelerationGround) / horizontalSpeed;
            moveVelocity.z *= (this.maxSpinJumpAirHorizontalSpeed * this.rateOfRunHorizontalSpeedOnAccelerationGround) / horizontalSpeed;
        }
        this._velocity = new Vector3(moveVelocity.x, this._velocity.y, moveVelocity.z);

        //vertical下方向に最高速度を越えていれば、最高速度にする
        if (this._velocity.y < -1 * this.maxSpinJumpAirVerticalSpeed) this._velocity.y = -1 * this.maxSpinJumpAirVerticalSpeed;

        //回転運動
        this.transform.Rotate(new Vector3(0f, 2000f, 0f) * Time.deltaTime, Space.World);

        if (!SEManager.Instance.GetCurrentAudioNames().Any(s => s == "JumpWind1")) SEManager.Instance.Play(SEPath.JUMP_WIND1, volumeRate: 0.5f);
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
        this._hitHeadCheck.localPosition = Vector3.zero;
        this._hitHeadCheck.localRotation = Quaternion.Euler(Vector3.zero);
        this._groundCheck.localPosition = Vector3.zero;
        this._groundCheck.localRotation = Quaternion.Euler(Vector3.zero);
        StartCoroutine(TransformManager.RotateInCertainTimeByFixedAxisFromAway(this.transform, this.CenterPosition, E_TransformAxis.Right, -1080f, 1f));
        StartCoroutine(TransformManager.RotateInCertainTimeByFixedAxisFromAway(this._hitHeadCheck, this.CenterPosition, E_TransformAxis.Right, 1080f, 1f));
        StartCoroutine(TransformManager.RotateInCertainTimeByFixedAxisFromAway(this._groundCheck, this.CenterPosition, E_TransformAxis.Right, 1080f, 1f));
        StartCoroutine(CoroutineManager.DelayMethod(1.1f, () =>
        {
            this._hitHeadCheck.localPosition = Vector3.zero;
            this._hitHeadCheck.localRotation = Quaternion.Euler(Vector3.zero);
            this._groundCheck.localPosition = Vector3.zero;
            this._groundCheck.localRotation = Quaternion.Euler(Vector3.zero);
        }));

        //this.canStickWall = false;
        //StartCoroutine(CoroutineManager.DelayMethod(0.3f, () => this.canStickWall = true));

        this.currentState = E_State.BackFliping;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.BackFlip);
        SEManager.Instance.Play(SEPath.JUMP_VOICE1);
        SEManager.Instance.Play(SEPath.JUMP_WIND0);
        StartCoroutine(CoroutineManager.DelayMethod(0.4f, () => SEManager.Instance.Play(SEPath.JUMP_WIND0)));
        StartCoroutine(CoroutineManager.DelayMethod(0.8f, () => SEManager.Instance.Play(SEPath.JUMP_WIND0)));
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
        SEManager.Instance.Play(SEPath.JUMP_VOICE7);
        SEManager.Instance.Play(SEPath.JUMP_WIND1);
    }

    /// <summary>
    /// ヒップドロップ。空中でvelocity0に。一瞬待って真下に等速運動
    /// </summary>
    private void HipDrop()
    {
        this.StopAllCoroutineOfRotation(); //現在はbackFlipのコルーチンのみ
        this._velocity = Vector3.zero;
        this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);
        this.currentState = E_State.HipDropping;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.HipDrop);
        SEManager.Instance.Stop(SEPath.STICKING_WALL);
        SEManager.Instance.Play(SEPath.JUMP_VOICE8);
        SEManager.Instance.Play(SEPath.HIP_DROP_ROTATE);
        CapsuleCollider _collider = GetComponent<CapsuleCollider>();
        this._hitHeadCheck.localPosition = Vector3.zero;
        this._hitHeadCheck.localRotation = Quaternion.Euler(Vector3.zero);
        this._groundCheck.localPosition = Vector3.zero;
        this._groundCheck.localRotation = Quaternion.Euler(Vector3.zero);
        StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this.transform, this.CenterPosition, E_TransformAxis.Right, 360f, 0.14f));
        StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this._hitHeadCheck, this.CenterPosition, E_TransformAxis.Right, -360f, 0.14f));
        StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this._groundCheck, this.CenterPosition, E_TransformAxis.Right, -360f, 0.14f));
        StartCoroutine(CoroutineManager.DelayMethod(0.15f, () =>
        {
            this._hitHeadCheck.localPosition = Vector3.zero;
            this._hitHeadCheck.localRotation = Quaternion.Euler(Vector3.zero);
            this._groundCheck.localPosition = Vector3.zero;
            this._groundCheck.localRotation = Quaternion.Euler(Vector3.zero);
        }));
        StartCoroutine(CoroutineManager.DelayMethod(0.3f, () =>
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

        this._isGrounded = false;
        this.currentState = E_State.JumpToTop;

        //移動させる
        this._velocity = new Vector3(this.NormalOfStickingWall.x * this.wallKickHorizontalSpeed + this._velocity.x, this.wallKickVerticalSpeed, this.NormalOfStickingWall.z * this.wallKickHorizontalSpeed + this._velocity.z);

        //回転
        //this.transform.forward = this.NormalOfStickingWall; //壁の法線ベクトルを向く
        this.transform.forward = new Vector3(this._velocity.x, 0f, this._velocity.z); //移動方向を向く

        //animation
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);

        SEManager.Instance.Stop(SEPath.STICKING_WALL);
        SEManager.Instance.Play(SEPath.JUMP_VOICE3);
        SEManager.Instance.Play(SEPath.JUMP_WIND0, volumeRate: 0.5f);

    }

    /// <summary>
    /// 壁に張り付く。張り付いたときに壁方向を向き、水平方向の速度を0にする
    /// </summary>
    /// <param name="normalOfStickingWall">壁面の垂直方向(自身のほうへの)</param>
    public void StickWall(Vector3 normalOfStickingWall)
    {
        if (this._isGrounded || this.currentState == E_State.HipDropping) return;
        if (this.currentState != E_State.HipDropping) this.StopAllCoroutineOfRotation();
        this._velocity = new Vector3(0f, this._velocity.y, 0f);
        this.transform.rotation = Quaternion.LookRotation(-1 * normalOfStickingWall, Vector3.up);
        this.NormalOfStickingWall = normalOfStickingWall;
        this.currentState = E_State.StickingWall;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.StickingWall);
        //this.stickingWallSmoke.Play(true);
    }

    /// <summary>
    /// 壁張り付き状態から、落下状態へ移行
    /// </summary>
    public void StopStickingWall()
    {
        if (this._isGrounded || this.currentState != E_State.StickingWall) return;

        this.currentState = E_State.Falling;
        this.NormalOfStickingWall = Vector3.zero;
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopToGround);
        SEManager.Instance.Stop(SEPath.STICKING_WALL);
        //this.stickingWallSmoke.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    /// <summary>
    /// 現在の速度に速度ベクトルを足す(移動量を足すのではない)
    /// </summary>
    /// <param name="addVelocity">足す速度(移動量ではない)</param>
    /// <param name="addInAir">プレイヤーが空中にいるときも足すかどうか</param>
    public void AddVelocity(Vector3 addVelocity, bool addInAir)
    {
        if (addInAir) this.addVelocityThisFrame += addVelocity;
        else this.addVelocityThisFrameInGrounded += addVelocity;
    }

    /// <summary>
    /// CharacterController.Moveを使って移動させる。ただし次のフレームで。
    /// </summary>
    /// <param name="addPosition">移動量</param>
    /// <param name="addInAir">空中時も移動するか</param>
    public void AddPosition(Vector3 addPosition, bool addInAir)
    {
        if (addInAir) this.addPositionThisFrame += addPosition;
        else this.addPositionThisFrameInGrounded += addPosition;
    }

    /// <summary>
    /// characterControllerを一時的にfalseにし、positionを書き換える
    /// </summary>
    /// <param name="addPosition">移動量</param>
    /// <param name="addInAir">空中時も移動するか</param>
    public void MovePositionImmediately(Vector3 addPosition, bool addInAir)
    {
        if(addInAir || (!addInAir && this._isGrounded))
        {
            this._characterController.enabled = false;
            this.transform.position += addPosition;
            this._characterController.enabled = true;
        }
    }

    /// <summary>
    /// 特定の座標にワープする
    /// </summary>
    /// <param name="toWarpPosition">ワープ先の座標</param>
    public void Warp(Vector3 toWarpPosition)
    {
        this._characterController.enabled = false;
        this.transform.position = toWarpPosition;
        this._characterController.enabled = true;
    }

    /// <summary>
    /// ステージに頭をぶつけたときに、垂直上方向の加速を消す
    /// </summary>
    public void HitHeadOnStage()
    {
        if (this._velocity.y > 3f) this._velocity.y = 3f;
        /*if(this.currentState == E_State.BackFliping)
        {
            this.currentState = E_State.Falling;
            StopAllCoroutineOfRotation();
        }*/
    }

    /// <summary>
    /// このクラス内のコルーチン処理を停止する(回転処理の停止)
    /// </summary>
    public void StopAllCoroutineOfRotation()
    {
        StopAllCoroutines();
        this.transform.localRotation = Quaternion.Euler(new Vector3(0f, this.transform.rotation.eulerAngles.y, 0f));
        this._hitHeadCheck.transform.localPosition = Vector3.zero;
        this._hitHeadCheck.transform.localRotation = Quaternion.Euler(Vector3.zero);
        this._groundCheck.transform.localPosition = Vector3.zero;
        this._groundCheck.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void StopAllMove(int stopFrame)
    {
        StageTimeManager.Instance.PlayerStop = true;
        StartCoroutine(CoroutineManager.DelayMethod(stopFrame, () => StageTimeManager.Instance.PlayerStop = false));
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
        //LongJumpToTop, //animationは普通のジャンプと変わらない(初速度だけ違う)→いらないかも
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
        //LongJump,
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
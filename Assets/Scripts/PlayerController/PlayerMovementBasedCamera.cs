using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// カメラの向きから相対的に移動する
/// </summary>
public class PlayerMovementBasedCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float gravityPower = 3f;
    private Rigidbody _rigidbody;
    private bool _isGrounded = false;
    private E_State currentState;

    private CharacterController _characterController;
    private PlayerAnimation _playerAnimation;

    private Vector3 addForceDownPower = Vector3.down;
    private Vector3 velocity;

    /// <summary>
    /// 壁に空中で張り付いているか(stateのひとつとして持ってもいい)
    /// </summary>
    public bool IsStickingWall { get; set; } = false;

    public bool IsGrounded => this._isGrounded;

    public PlayerAnimation _PlayerAnimation => this._playerAnimation;

    private void Awake()
    {
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
        //MoveByCameraPosition();
        MoveTest();
        PlayAnimationOfPlayer();
    }

    private void MoveTest()
    {
        if (this._isGrounded)
        {
            this.velocity = Vector3.zero;

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
            this.velocity = moveForward * this.moveSpeed + new Vector3(0f, this.velocity.y, 0f);

            if (moveForward != Vector3.zero)
            {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveForward), 0.2f);
            }

            if (moveForward.magnitude > 0f) this.currentState = E_State.Running;
            else this.currentState = E_State.Standing;

            if (Input.GetButtonDown("Jump"))
            {
                this.currentState = E_State.JumpToTop;
                velocity.y += jumpForce;
                this._isGrounded = false;
            }
            else
            {
                this.velocity += this.addForceDownPower;
            }
        }
        else
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
            this.velocity += moveForward * this.moveSpeed * 0.01f; //この場合、入力している間加速し続けるため、最大速度の設定が必要。

            if (this.velocity.y < -0.05f) this.currentState = E_State.Falling;
            else this.currentState = E_State.TopOfJump;
        }

        velocity.y -= this.gravityPower * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);

        this._playerAnimation.PlayerAnimator.SetBool("IsLanding", this._isGrounded);
    }

    private void MoveByCameraPosition()
    {
        //debugよう
        //this._isGrounded = true;

        if (this._isGrounded)
        {
            this.velocity = Vector3.zero;
        }
        else
        {
            //Debug.Log("uiteru");
            this.velocity = new Vector3(0f, this.velocity.y, 0f);
        }

        //移動
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
        this.velocity = moveForward * this.moveSpeed + new Vector3(0f, this.velocity.y, 0f);

        //回転
        if (moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveForward), 0.2f);
        }

        //animation
        if (this._isGrounded)
        {
            if (moveForward.magnitude > 0f) this.currentState = E_State.Running;
            else this.currentState = E_State.Standing;
        }
        else
        {
            if (this.velocity.y < -0.05f) this.currentState = E_State.Falling;
            else this.currentState = E_State.TopOfJump;
        }

        //jump
        if (this._isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                this.currentState = E_State.JumpToTop;
                velocity.y += jumpForce;
                this._isGrounded = false;
            }
            else
            {
                this.velocity += this.addForceDownPower;
            }
        }
        
        /*else
        {
            velocity += this.addForceDownPower;
        }*/

        velocity.y -= this.gravityPower * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);

        this._playerAnimation.PlayerAnimator.SetBool("IsLanding", this._isGrounded);
    }

    private void PlayAnimationOfPlayer()
    {

        switch (this.currentState)
        {
            case E_State.Standing:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.Standing);
                break;
            case E_State.Running:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.Running);
                break;
            case E_State.JumpToTop:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
                break;
            case E_State.Falling:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopToGround);
                break;
            case E_State.TopOfJump:
                this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopOfJump);
                break;
        }
    }

    /// <summary>
    /// 壁キック
    /// </summary>
    /// <param name="normalOfWall">張り付いている壁の法線ベクトル</param>
    public void WallKick(Vector3 normalOfWall)
    {
        if (this.velocity.y >= -0.05f) return;
        Debug.Log("壁キック");
        //向きを変える(とりあえず現在の向きを反転で妥協)
        //this.transform.rotation = Quaternion.Euler(Vector3.Reflect(this.transform.rotation.eulerAngles, normalOfWall));
        //this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        //法線ベクトルのyの大きさに制限を加える？
        //this.transform.LookAt(this.transform.position + normalOfWall);
        this.transform.forward = normalOfWall;

        //移動させる
        this.velocity = new Vector3(normalOfWall.x * 5f, 50f, normalOfWall.z * 5f);

        //animation
        this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.JumpToTop);
    }

    public void StickWall(bool startStick)
    {
        if (this._isGrounded) return;
        if (startStick)
        {
            this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.StickingWall);
        }
        else
        {
            this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.TopToGround);
        }
    }

    public enum E_State
    {
        Standing,
        Running,
        JumpToTop,
        TopOfJump,
        Falling,
        Damaged,
        Salute,
        KnellDown,
        Other
    }
}

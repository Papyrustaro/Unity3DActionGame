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
        MoveByCameraPosition();
        PlayAnimationOfPlayer();
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
        //Vector3 moveForward = cameraForward * this.inputVertical + Camera.main.transform.right * this.inputHorizontal;
        Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
        //this._rigidbody.velocity = moveForward * this.moveSpeed + new Vector3(0, this._rigidbody.velocity.y, 0);
        this.velocity = moveForward * this.moveSpeed + new Vector3(0f, this.velocity.y, 0f);

        //回転
        if (moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveForward), 0.2f);
        }

        //animation
        if (this._isGrounded)
        {
            //this._playerAnimation.PlayerAnimator.SetBool("IsLanding", true);
            if (moveForward.magnitude > 0f) this.currentState = E_State.Running;
            else this.currentState = E_State.Standing;
        }
        else
        {
            //this._playerAnimation.PlayerAnimator.SetBool("IsLanding", false);
            if (this.velocity.y < -0.05f) this.currentState = E_State.Falling;
            else this.currentState = E_State.TopOfJump;
        }

        //jump
        if (this._isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                this.currentState = E_State.JumpToTop;
                //this._rigidbody.AddForce(Vector3.up * this.jumpForce);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stage"))
        {
            this._isGrounded = true;
            //this._playerAnimation.Play(PlayerAnimation.E_PlayerAnimationType.Standing);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stage"))
        {
            Debug.Log("消えた");
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

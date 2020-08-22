using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class WallKickTrigger : MonoBehaviour
{
    private PlayerMovementBasedCamera playerMoveController;
    private BoxCollider _collider;
    private MonobitEngine.MonobitView _monobitView;
    private int stickWallCount = 0;
    public bool IsStickingOnDisappearableWall { get; set; } = false;
    private Vector3 normalVector = Vector3.zero;

    private void Awake()
    {
        this._monobitView = this.transform.parent.GetComponent<MonobitEngine.MonobitView>();
        this.playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
        this._collider = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!StageTimeManager.Instance.IsPlayerMoving) return;
        if (this.playerMoveController.IsGrounded && this._collider.enabled) {this._collider.enabled = false; this.stickWallCount = 0; this.normalVector = Vector3.zero; }
        if (!this.playerMoveController.IsGrounded && !this._collider.enabled) StartCoroutine(CoroutineManager.DelayMethod(10, () => this._collider.enabled = true));
        CheckInputToWall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround") || collision.transform.CompareTag("SwitchOnOffStage"))
        {
            Vector3 normalVector = collision.contacts[0].normal;
            float angle = Vector3.Angle(new Vector3(normalVector.x, 0f, normalVector.z), new Vector3(this.transform.forward.x, 0f, this.transform.forward.z));
            /*if (normalVector.y < 0.02f && 100f <= angle) //入射角が80°以内ではりつく
            {*/
            if(!(SceneManager.GetActiveScene().name != "Stage2" && normalVector.y > 0.02))
            {
                //if (this.stickWallCount == 0) this.playerMoveController.StickWall(normalVector);
                if (this.stickWallCount == 0) { this.normalVector = normalVector; this.stickWallCount++; };
                //this.stickWallCount++;
            }
            if (collision.transform.CompareTag("SwitchOnOffStage"))
            {
                this.IsStickingOnDisappearableWall = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround") || collision.transform.CompareTag("SwitchOnOffStage"))
        {
            AwayFromOneWall();
        }
    }

    private void CheckInputToWall()
    {
        if (this.playerMoveController.IsStickingWall || this.normalVector == Vector3.zero) return;
        Vector2 inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputVelocity == Vector2.zero) return;
        //カメラの角度に合わせて入力方向に移動
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * inputVelocity.y + Camera.main.transform.right * inputVelocity.x;
        float angle = Vector3.Angle(this.normalVector, moveForward);
        //Debug.Log(angle);
        if(angle > 120f)
        {
            this.playerMoveController.StickWall(this.normalVector);
        }

    }

    public void AwayFromOneWall()
    {
        this.stickWallCount--;
        if (this.stickWallCount < 0) this.stickWallCount = 0;
        if (this.stickWallCount == 0)
        {
            this.normalVector = Vector3.zero;
            this.playerMoveController.StopStickingWall();
        }
    }

    public void ResetTrigger()
    {
        this.stickWallCount = 0;
        this._collider.enabled = false;
        StartCoroutine(CoroutineManager.DelayMethod(8, () => this._collider.enabled = true));
    }
}

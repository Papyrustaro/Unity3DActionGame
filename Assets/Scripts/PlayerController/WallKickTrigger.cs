using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallKickTrigger : MonoBehaviour
{
    private PlayerMovementBasedCamera playerMoveController;
    private BoxCollider _collider;
    private MonobitEngine.MonobitView _monobitView;
    private int stickWallCount = 0;

    private void Awake()
    {
        this._monobitView = this.transform.parent.GetComponent<MonobitEngine.MonobitView>();
        this.playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
        this._collider = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!StageTimeManager.Instance.IsPlayerMoving) return;
        if (this.playerMoveController.IsGrounded && this._collider.enabled) {this._collider.enabled = false; this.stickWallCount = 0; }
        if (!this.playerMoveController.IsGrounded && !this._collider.enabled) StartCoroutine(CoroutineManager.DelayMethod(10, () => this._collider.enabled = true)); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround"))
        {
            Vector3 normalVector = collision.contacts[0].normal;
            float angle = Vector3.Angle(new Vector3(normalVector.x, 0f, normalVector.z), new Vector3(this.transform.forward.x, 0f, this.transform.forward.z));
            if (normalVector.y < 0.02f && 100f <= angle) //入射角が80°以内ではりつく
            {
                //Debug.Log(Vector3.Angle(new Vector3(normalVector.x, 0f, normalVector.z), new Vector3(this.transform.forward.x, 0f, this.transform.forward.z)));
                if(this.stickWallCount == 0) this.playerMoveController.StickWall(normalVector);
                this.stickWallCount++;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround"))
        {
            this.stickWallCount--;
            if (this.stickWallCount < 0) this.stickWallCount = 0;
            if(this.stickWallCount == 0) this.playerMoveController.StopStickingWall();
        }
    }

    public void ResetTrigger()
    {
        this.stickWallCount = 0;
        this._collider.enabled = false;
        StartCoroutine(CoroutineManager.DelayMethod(8, () => this._collider.enabled = true));
    }
}

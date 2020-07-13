using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallKickTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovementBasedCamera playerMoveController;
    private BoxCollider _collider;
    private MonobitEngine.MonobitView _monobitView;
    private int stickWallCount = 0;

    private void Awake()
    {
        this._monobitView = this.transform.root.GetComponent<MonobitEngine.MonobitView>();
        this._collider = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (this.playerMoveController.IsGrounded && this._collider.enabled) {this._collider.enabled = false; this.stickWallCount = 0; }
        if (!this.playerMoveController.IsGrounded && !this._collider.enabled) StartCoroutine(CoroutineManager.DelayMethod(10, () => this._collider.enabled = true)); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enter: " + this.stickWallCount);
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround"))
        {
            Vector3 normalVector = collision.contacts[0].normal;
            if(normalVector.y < 0.02f) //壁が一定以上垂直方向から傾いていなければ張り付く
            {
                if(this.stickWallCount == 0) this.playerMoveController.StickWall(normalVector);
                this.stickWallCount++;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit: " + this.stickWallCount);
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround"))
        {
            this.stickWallCount--;
            if (this.stickWallCount < 0) this.stickWallCount = 0;
            if(this.stickWallCount == 0) this.playerMoveController.StopStickingWall();
        }
    }
}

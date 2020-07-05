using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKickTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovementBasedCamera playerMoveController;
    private MonobitEngine.MonobitView _monobitView;

    private void Awake()
    {
        this._monobitView = this.transform.root.GetComponent<MonobitEngine.MonobitView>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround"))
        {
            Vector3 normalVector = collision.contacts[0].normal;
            if(normalVector.y < 0.02f) //壁が一定以上垂直方向から傾いていなければ張り付く
            {
                this.playerMoveController.StickWall(normalVector);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (this._monobitView != null && !this._monobitView.isMine) return;
        if (collision.transform.CompareTag("Stage") || collision.transform.CompareTag("MoveStage") || collision.transform.CompareTag("AccelerationGround"))
        {
            this.playerMoveController.StopStickingWall();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKickTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovementBasedCamera playerMoveController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stage"))
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
        if (collision.transform.CompareTag("Stage"))
        {
            this.playerMoveController.StopStickingWall();
        }
    }
}

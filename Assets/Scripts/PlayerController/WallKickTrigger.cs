using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKickTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovementBasedCamera playerMoveController;

    /// <summary>
    /// 張り付いている壁の法線ベクトル
    /// </summary>
    private Vector3 normalOfWall;

    /*private void Update()
    {
        if(this.normalOfWall != Vector3.zero && !this.playerMoveController.IsGrounded)
        {
            //this.playerMoveController.InitVelocity(E_Vector.XZ);
            this.playerMoveController.ChangeVelocityByRate(E_Vector.XZ, 0.2f);
            if (Input.GetButtonDown("Jump"))
            {
                //this.playerMoveController.WallKick(this.normalOfWall);
            }
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stage"))
        {
            //this.normalOfWall = collision.contacts[0].normal;
            this.playerMoveController.NormalOfStickingWall = collision.contacts[0].normal;
            this.playerMoveController.StickWall(true);
            //this.playerMoveController.StickWall(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Stage"))
        {
            this.playerMoveController.NormalOfStickingWall = Vector3.zero;
            this.playerMoveController.StickWall(false);
            //this.normalOfWall = Vector3.zero;
            //this.playerMoveController.StickWall(false);
            //this.playerMoveController._PlayerAnimation.PlayerAnimator.SetBool("StickingWall", false);
            //this.playerMoveController.IsStickingWall = false;
        }
    }
}

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

    private void Update()
    {
        if (this.normalOfWall != Vector3.zero && Input.GetButtonDown("Jump") && !this.playerMoveController.IsGrounded)
        {
            this.playerMoveController.WallKick(this.normalOfWall);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stage"))
        {
            this.normalOfWall = collision.contacts[0].normal;
            Debug.Log(this.normalOfWall);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Stage"))
        {
            this.normalOfWall = Vector3.zero;
            this.playerMoveController.IsStickingWall = false;
        }
    }
}

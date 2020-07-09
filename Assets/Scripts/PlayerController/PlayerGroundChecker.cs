using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの接地判定処理(判定はそこそこ厳密)
/// </summary>
public class PlayerGroundChecker : MonoBehaviour
{
    private PlayerMovementBasedCamera playerMoveController;

    private void Awake()
    {
        this.playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AccelerationGround"))
        {
            this.playerMoveController.IsOnAccelerationGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AccelerationGround"))
        {
            this.playerMoveController.IsOnAccelerationGround = false;
        }
    }
}

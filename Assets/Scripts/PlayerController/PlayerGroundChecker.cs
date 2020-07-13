using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの接地判定処理(判定はそこそこ厳密)
/// </summary>
public class PlayerGroundChecker : MonoBehaviour
{
    private PlayerMovementBasedCamera playerMoveController;
    private int accelerationGroundColliderCount = 0;

    private void Awake()
    {
        this.playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AccelerationGround"))
        {
            this.accelerationGroundColliderCount++;
            this.playerMoveController.IsOnAccelerationGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AccelerationGround"))
        {
            this.accelerationGroundColliderCount--;
            if(this.accelerationGroundColliderCount == 0) this.playerMoveController.IsOnAccelerationGround = false;
        }
    }
}

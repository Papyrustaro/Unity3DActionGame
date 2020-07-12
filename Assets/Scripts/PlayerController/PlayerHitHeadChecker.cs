using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHeadChecker : MonoBehaviour
{
    private PlayerMovementBasedCamera _playerMoveController;

    private void Awake()
    {
        this._playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //衝突したgameObjectのlayerがStageだったら
        if(other.gameObject.layer == 8)
        {
            this._playerMoveController.HitHeadOnStage();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 範囲にプレイヤーが入ると、ワープ先へとワープする
/// </summary>
public class WarpZone : MonoBehaviour
{
    [SerializeField] private Transform toWarpPosition;
    [SerializeField] private UnityEvent onWarp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.onWarp.Invoke();
            if(SceneManager.GetActiveScene().name == "Title")
            {
                other.GetComponent<PlayerMovementBasedCamera>().Warp(this.toWarpPosition.position);
            }
            else
            {
                toWarpPosition.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<PlayerMovementBasedCamera>().Warp(this.toWarpPosition.position);
            }

            if(SwitchOnOffStageInHitHeadController.Instance != null)
            {
                SwitchOnOffStageInHitHeadController.Instance.SwitchAllOnWarp();
            }
        }
    }

}

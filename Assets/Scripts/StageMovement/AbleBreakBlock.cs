using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

/// <summary>
/// 壊すことができるブロック
/// </summary>
public class AbleBreakBlock : MonoBehaviour
{
    [SerializeField] private GameObject brokenEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitHeadCheck"))
        {
            if (!other.transform.parent.GetComponent<PlayerMovementBasedCamera>().IsGrounded)
            {
                BreakMe();
            }
        }
        if (other.CompareTag("PlayerGroundCheck"))
        {
            PlayerMovementBasedCamera playerController = other.transform.parent.GetComponent<PlayerMovementBasedCamera>();
            if(playerController.CurrentState == PlayerMovementBasedCamera.E_State.HipDropping && playerController.AbleBreakByHipDrop)
            {
                playerController.AbleBreakByHipDrop = false;
                //playerController.StopAllMove(2);
                BreakMe();
            }
        }
    }

    public void BreakMe()
    {
        SEManager.Instance.Play(SEPath.BREAK_CUBE, 0.8f);
        Instantiate(this.brokenEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}

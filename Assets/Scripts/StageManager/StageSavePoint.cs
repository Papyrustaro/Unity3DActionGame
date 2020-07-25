using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSavePoint : MonoBehaviour
{
    [SerializeField] private Transform rebornPosition;
    [SerializeField] private PlayerMovementBasedCamera playerController;
    private bool isPointReached = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.isPointReached = true;
        }
    }

    public void WarpPoint()
    {
        if (this.isPointReached)
        {
            this.playerController.Warp(this.rebornPosition.position);
        }
    }
}

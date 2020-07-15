﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            other.GetComponent<PlayerMovementBasedCamera>().Warp(this.toWarpPosition.position + other.transform.forward * 1f);
        }
    }
}

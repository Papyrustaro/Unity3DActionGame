﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動床用の座標チェック(判定は大きめにしている→移動速度が速いと1フレームで抜けてしまうため)
/// </summary>
public class PlayerMoveGroundChecker : MonoBehaviour
{
    private PlayerMovementBasedCamera playerMoveController;
    private Transform groundedMoveStageTransform;
    private Vector3 moveStagePositionBeforeFrame;
    private Vector3 moveStagePositionCurrentFrame;
    [SerializeField] private bool jumpWithVelocity = false; //移動床上でジャンプしたときに床の移動速度を追加するか。しないでいく予定


    private void Awake()
    {
        this.playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
    }

    private void Update()
    {
        if (!StageTimeManager.Instance.IsPlayerMoving) return;
        if(this.groundedMoveStageTransform && StageTimeManager.Instance.IsStageMoving)
        {
            this.moveStagePositionCurrentFrame = this.groundedMoveStageTransform.position;
            if (this.jumpWithVelocity) this.playerMoveController.AddVelocity((this.moveStagePositionCurrentFrame - this.moveStagePositionBeforeFrame) / Time.deltaTime, false);
            else this.playerMoveController.MovePositionImmediately(this.moveStagePositionCurrentFrame - this.moveStagePositionBeforeFrame + Vector3.down * 0.05f, false);
            this.moveStagePositionBeforeFrame = this.moveStagePositionCurrentFrame;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoveStage"))
        {
            this.groundedMoveStageTransform = other.transform;
            this.moveStagePositionBeforeFrame = this.groundedMoveStageTransform.position;
        }
        if(other.CompareTag("Stage") || other.CompareTag("MoveStage") || other.CompareTag("AccelerationGround") || other.CompareTag("SwitchOnOff") || other.CompareTag("SwitchOnOffStage"))
        {
            this.playerMoveController.CountGroundNearFoots++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MoveStage"))
        {
            if(other.transform == this.groundedMoveStageTransform) this.groundedMoveStageTransform = null;
        }
        if (other.CompareTag("Stage") || other.CompareTag("MoveStage") || other.CompareTag("AccelerationGround") || other.CompareTag("SwitchOnOff") || other.CompareTag("SwitchOnOffStage"))
        {
            if (this.playerMoveController.CountGroundNearFoots > 0) this.playerMoveController.CountGroundNearFoots--;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerGroundChecker : MonoBehaviour
{
    private PlayerMovementBasedCamera playerMoveController;
    private Transform groundedMoveStageTransform;
    private Vector3 moveStagePositionBeforeFrame;
    private Vector3 moveStagePositionCurrentFrame;


    private void Awake()
    {
        this.playerMoveController = this.transform.parent.GetComponent<PlayerMovementBasedCamera>();
    }

    private void Update()
    {
        if(this.groundedMoveStageTransform != null)
        {
            //Debug.Log(this.moveStagePositionCurrentFrame - this.moveStagePositionBeforeFrame);
            //Debug.Log(this.moveStagePositionCurrentFrame.y - this.moveStagePositionBeforeFrame.y == 0);
            this.moveStagePositionCurrentFrame = this.groundedMoveStageTransform.position;
            this.playerMoveController.AddVelocity(this.moveStagePositionCurrentFrame - this.moveStagePositionBeforeFrame);
            //this.playerMoveController.transform.position = this.playerMoveController.transform.position + (this.moveStagePositionCurrentFrame - this.moveStagePositionBeforeFrame);
            this.moveStagePositionBeforeFrame = this.moveStagePositionCurrentFrame;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("MoveStage"))
        {
            Debug.Log("B");
            this.groundedMoveStageTransform = other.transform;
            this.moveStagePositionBeforeFrame = this.groundedMoveStageTransform.position;
            //this.playerTransform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MoveStage"))
        {
            Debug.Log("C");
            this.groundedMoveStageTransform = null;
            //this.playerTransform.SetParent(null);
        }
    }
}

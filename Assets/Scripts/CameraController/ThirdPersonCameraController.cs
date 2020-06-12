using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの背後を基本とし、カメラを移動させる
/// </summary>
public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    private Vector3 targetPositionBeforeFrame;
    private void Start()
    {
        this.targetPositionBeforeFrame = this.targetPlayer.position;
    }

    private void Update()
    {
        this.transform.position += this.targetPlayer.position - this.targetPositionBeforeFrame;
        this.targetPositionBeforeFrame = this.targetPlayer.position;

        if (Input.GetMouseButton(1))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            this.transform.RotateAround(this.targetPositionBeforeFrame, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            this.transform.RotateAround(this.targetPositionBeforeFrame, this.transform.right, mouseInputY * Time.deltaTime * 200f);
        }
    }
}

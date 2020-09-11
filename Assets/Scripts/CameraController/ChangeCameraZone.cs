using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraZone : MonoBehaviour
{
    //[SerializeField] CinemachineVirtualCamera toChangeCamera;
    [SerializeField] private StageCamera toChangeCamera;

    private void OnTriggerEnter(Collider other)
    {
        //if (StageCameraManager.Instance.CurrentCamera == this.toChangeCamera.Camera) Debug.Log("同じカメラ");
        if (StageCameraManager.Instance.CurrentCamera != this.toChangeCamera.Camera && other.CompareTag("Player"))
        {
            StageCameraManager.Instance.SetCurrentCamera(this.toChangeCamera.Camera);
            ThirdPersonCameraController.Instance.SetDefaultCameraRotation(this.toChangeCamera.DefaultRotation);
        }
    }
}

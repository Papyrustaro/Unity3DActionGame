using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private ThirdPersonCameraController cameraController;

    public static StageCameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else throw new System.Exception();
    }

    public void SetAbleFollow(bool canFollow)
    {
        this.virtualCamera.enabled = canFollow;
    }

    public void SetAbleRotateByInput(bool canRotate)
    {
        this.cameraController.IsMoving = canRotate;
    }
}

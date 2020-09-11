using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstCamera;
    private CinemachineBrain cinemachineBrain;

    public static StageCameraManager Instance { get; private set; }

    //public StageCamera CurrentCamera { get; private set; }
    public CinemachineVirtualCamera CurrentCamera { get; private set; }

    public int CurrentPriority { get; set; } = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else throw new System.Exception();
        this.cinemachineBrain = GetComponent<CinemachineBrain>();
        this.CurrentCamera = this.firstCamera;
        ThirdPersonCameraController.Instance.SetDefaultCameraRotation(this.firstCamera.transform.rotation);
    }

    public void SetAbleFollow(bool canFollow)
    {
        this.cinemachineBrain.enabled = canFollow;
        //this.CurrentCamera.enabled = canFollow;
    }

    public void SetAbleRotateByInput(bool canRotate)
    {
        ThirdPersonCameraController.Instance.IsMoving = canRotate;
    }

    public void SetCurrentCamera(CinemachineVirtualCamera setCamera)
    {
        this.CurrentPriority++;
        setCamera.Priority = this.CurrentPriority;
        this.CurrentCamera = setCamera;
    }
}

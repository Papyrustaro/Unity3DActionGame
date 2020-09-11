using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageCamera : MonoBehaviour
{
    public CinemachineVirtualCamera Camera { get; private set; }
    public Quaternion DefaultRotation { get; private set; }

    private void Awake()
    {
        this.Camera = GetComponent<CinemachineVirtualCamera>();
        this.DefaultRotation = this.transform.rotation;
    }
}

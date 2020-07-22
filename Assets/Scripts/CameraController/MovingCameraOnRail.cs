using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovingCameraOnRail : MonoBehaviour
{

    private CinemachineTrackedDolly _dolly;

    private void Awake()
    {
        this._dolly = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void Update()
    {
        this._dolly.m_PathPosition += Time.deltaTime * 0.003f;
        if (this._dolly.m_PathPosition > 1f) this._dolly.m_PathPosition -= 1f;
    }
}

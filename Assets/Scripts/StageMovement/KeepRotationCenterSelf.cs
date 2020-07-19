using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class KeepRotationCenterSelf : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float rotateSpeed = 100f;


    private void Update()
    {
        if (StageTimeManager.Instance.IsStageMoving)
        {
            this.transform.RotateAround(this.transform.position, this.axis, this.rotateSpeed * Time.deltaTime);
        }
    }

    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void SetAxisSelfUp()
    {
        this.axis = this.transform.up;
    }

    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void SetAxisSelfRight()
    {
        this.axis = this.transform.right;
    }

    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void SetAxisSelfForward()
    {
        this.axis = this.transform.forward;
    }
}

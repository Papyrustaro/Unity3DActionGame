using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class KeepRotationCenterSelf : MonoBehaviour
{
    [SerializeField] private E_Direction axisDirection = E_Direction.Other;
    [SerializeField] private Vector3 axis;
    [SerializeField] private float rotateSpeed = 100f;

    private void Update()
    {
        if (StageTimeManager.Instance.IsStageMoving)
        {
            switch (this.axisDirection)
            {
                case E_Direction.Forward:
                    this.transform.RotateAround(this.transform.position, this.transform.forward, this.rotateSpeed * Time.deltaTime);
                    break;
                case E_Direction.Right:
                    this.transform.RotateAround(this.transform.position, this.transform.right, this.rotateSpeed * Time.deltaTime);
                    break;
                case E_Direction.Up:
                    this.transform.RotateAround(this.transform.position, this.transform.up, this.rotateSpeed * Time.deltaTime);
                    break;
                case E_Direction.Other:
                    this.transform.RotateAround(this.transform.position, this.axis, this.rotateSpeed * Time.deltaTime);
                    break;
            }
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

    public enum E_Direction
    {
        Up,
        Right,
        Forward,
        Other
    }
}

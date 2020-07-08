using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private Vector3 rotateCenterPosition;
    [SerializeField] private float rotateSpeed = 100f;

    private void Update()
    {
        if (StageTimeManager.Instance.IsStageMoving)
        {
            this.transform.RotateAround(this.rotateCenterPosition, this.axis, this.rotateSpeed * Time.deltaTime);
        }
    }
}

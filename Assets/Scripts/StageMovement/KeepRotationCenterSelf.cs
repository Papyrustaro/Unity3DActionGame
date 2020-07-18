using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

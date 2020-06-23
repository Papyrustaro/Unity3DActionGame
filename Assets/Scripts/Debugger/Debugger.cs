using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private float rotateUpSpeed = 50f;
    [SerializeField] private float rotateRightSpeed = 10f;
    //[SerializeField] private Transform axisUpObject;
    //[SerializeField] private Transform axisRightObject;
    //[SerializeField] private Transform rotateObject;
    [SerializeField] private bool rotateFlag = false;
    [SerializeField] private bool isCoroutine = false;

    private Vector3 rightAxis;

    private void Start()
    {
        //Debug.Log(this.transform.up);
        this.rightAxis = this.transform.right;
        if (this.isCoroutine)
        {
            StartCoroutine(TransformManager.RotateInCertainTimeByFixedAxisFromAway(this.transform, this.transform, E_TransformAxis.Right, 360f * 100f * this.rotateRightSpeed, 100f));
            StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this.transform, this.transform, E_TransformAxis.Up, 360f * 100f * this.rotateUpSpeed, 100f));
        }
    }
    private void Update()
    {
        //Debug.Log(TransformManager.GetAxis(this.transform, E_TransformAxis.Up).ToString());
        //this.transform.rotation *= Quaternion.Euler(0, this.rotateSpeed * Time.deltaTime, 0f);
        //this.transform.rotation *= Quaternion.Euler(this.rotateSpeed * Time.deltaTime, 0f, 0f);
        //this.transform.rotation *= Quaternion.Euler(this.rotateRightSpeed * Time.deltaTime, this.rotateSpeed * Time.deltaTime, 0f);
        //this.transform.Rotate(Vector3.up * this.rotateUpSpeed * Time.deltaTime, Space.Self);
        //this.transform.Rotate(this.rightAxis * this.rotateRightSpeed * Time.deltaTime, Space.Self);

        /*正常にできる*/
        if (!this.isCoroutine)
        {
            this.transform.RotateAround(this.transform.position, this.transform.up, 360f * Time.deltaTime * this.rotateUpSpeed);
            if (this.rotateFlag) this.transform.RotateAround(this.transform.position, rightAxis, 360f * Time.deltaTime * this.rotateRightSpeed);
        }
        /**/

        //StartCoroutine(TransformManager.RotateInCertainTimeByFixedAxisFromAway(this.transform, this.transform, E_TransformAxis.Right, 360f * 10f, 10f));
        //StartCoroutine(TransformManager.RotateInCertainTimeByAxisFromAway(this.transform, this.transform, E_TransformAxis.Up, 360f * 10f, 10f));

        //Debug.Log(this.transform.up);

        //this.transform.Rotate(Vector3.up * this.rotateSpeed * Time.deltaTime, Space.Self);
        //this.transform.Rotate(Vector3.right * this.rotateSpeed * Time.deltaTime, Space.Self);
        //this.transform.Rotate(Vector3.right, this.rotateSpeed * Time.deltaTime);
    }
}

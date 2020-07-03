using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAndBackMovement : MonoBehaviour
{
    //private float xLength = 3f;
    private float moveSpeed = 3f;

    private void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + this.moveSpeed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
    }
}

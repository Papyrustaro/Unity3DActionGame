using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPlayerSample : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveForce = 1f;
    private Rigidbody _rigidbody;
    private Vector3 diff;

    private void Awake()
    {
        this._rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.diff = new Vector3(this.target.position.x - this.transform.position.x, this.target.position.y - this.transform.position.y, this.target.position.z - this.transform.position.z).normalized;
    }

    private void FixedUpdate()
    {
        this._rigidbody.AddForce(this.diff * this.moveForce);
    }

}
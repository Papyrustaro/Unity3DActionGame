using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float boundPower = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {   
            other.transform.parent.GetComponent<PlayerMovementBasedCamera>().Jump(this.boundPower);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PlayerGroundCheck"))
        {
            collision.transform.GetComponent<PlayerMovementBasedCamera>().Jump(this.boundPower);
        }
    }*/
}

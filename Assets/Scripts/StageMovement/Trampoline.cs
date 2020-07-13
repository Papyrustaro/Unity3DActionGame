using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private const float boundPower = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            other.transform.parent.GetComponent<PlayerMovementBasedCamera>().Jump(boundPower);
        }
    }
}

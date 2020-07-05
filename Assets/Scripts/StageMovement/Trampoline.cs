using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float boundPower = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            other.GetComponent<PlayerMovementBasedCamera>().Jump(this.boundPower);
        }
    }
}

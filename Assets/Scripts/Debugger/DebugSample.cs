using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugSample : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Debug.Log(other.gameObject.name);
    }
}

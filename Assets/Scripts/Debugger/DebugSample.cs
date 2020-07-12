using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugSample : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(CoroutineManager.DelayMethod(3f, () => Debug.Log("3")));
        StartCoroutine(CoroutineManager.DelayMethod(5f, () => Debug.Log("5")));
        StartCoroutine(CoroutineManager.DelayMethod(10f, () => Debug.Log("10")));

    }
}

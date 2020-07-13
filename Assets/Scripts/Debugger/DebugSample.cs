using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugSample : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(CoroutineManager.DelayMethod(3f, () => Debug.Log("C")));

    }
}

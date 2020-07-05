using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoroutineManager : MonoBehaviour
{
    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public static IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (int i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
}

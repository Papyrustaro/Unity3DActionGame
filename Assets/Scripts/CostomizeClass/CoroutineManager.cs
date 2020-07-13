using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoroutineManager : MonoBehaviour
{
    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        float countTime = 0f;
        while(countTime < waitTime)
        {
            if (!StageTimeManager.Instance.AllStop) countTime += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(waitTime);
        action();
    }

    public static IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (int i = 0; i < delayFrameCount; i++)
        {
            yield return null;
            if (StageTimeManager.Instance.AllStop) i--;
        }
        action();
    }
}

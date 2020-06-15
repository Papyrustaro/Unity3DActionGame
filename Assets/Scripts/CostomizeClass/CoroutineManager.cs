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

    public static IEnumerator OneRotationInCertainTime(Transform obj, Vector3 vec, float time)
    {
        float countTime = 0f;
        //float angle = 360f;
        Quaternion beforeRotation = obj.rotation;
        while (countTime < time)
        {
            obj.Rotate(vec * Time.deltaTime / time, Space.Self);
            //obj.Rotate(vec * Time.deltaTime / time);
            //obj.Rotate(Vector3.right, angle * Time.deltaTime / time);
            yield return null;
            countTime += Time.deltaTime;
        }

        obj.rotation = beforeRotation;
        yield break;
    }
}

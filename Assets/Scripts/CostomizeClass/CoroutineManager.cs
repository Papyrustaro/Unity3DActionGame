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

    public static IEnumerator OneRotationInCertainTime(Transform obj, Vector3 centerPositionFromTransform, Vector3 axis, float time, bool moveCameraInRotation)
    {
        //カメラの追従を一時停止
        if (!moveCameraInRotation) ThirdPersonCameraController.Instance.IsMoving = false;

        Vector3 centerOfRotation = obj.position + centerPositionFromTransform;
        float countTime = 0f;
        Quaternion beforeRotation = obj.rotation;

        //回転処理
        while (countTime < time)
        {
            obj.RotateAround(centerOfRotation, axis, 360f * Time.deltaTime / time);
            yield return null;
            countTime += Time.deltaTime;
        }

        //rotationとcameraの設定を回転前に戻して終了
        obj.rotation = beforeRotation;
        if (!moveCameraInRotation) ThirdPersonCameraController.Instance.IsMoving = true;
        yield break;
    }
}

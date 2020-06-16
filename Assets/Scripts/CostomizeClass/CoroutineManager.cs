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

    /// <summary>
    /// 特定の軸でobjを1回転させる
    /// </summary>
    /// <param name="obj">回転させるGameObjectのTransform</param>
    /// <param name="centerPositionFromTransform">Transform上の中心から、回転の中心にしたい座標までのベクトル</param>
    /// <param name="axis">回転する軸</param>
    /// <param name="time">一回転するのに用する時間(秒)</param>
    /// <param name="moveCameraInRotation">回転中もカメラをプレイヤーに合わせて動かすか</param>
    /// <returns></returns>
    public static IEnumerator OneRotationInCertainTime(Transform obj, Vector3 centerPositionFromTransform, Vector3 axis, float time, bool moveCameraInRotation)
    {
        Quaternion beforeRotation = obj.rotation;
        //カメラの追従を一時停止
        if (!moveCameraInRotation) ThirdPersonCameraController.Instance.IsMoving = false;

        Vector3 centerOfRotation = obj.position + centerPositionFromTransform;
        float countTime = 0f;

        //回転処理
        while (countTime < time)
        {
            countTime += Time.deltaTime;
            obj.RotateAround(centerOfRotation, axis, 360f * Time.deltaTime / time);
            yield return null;
            //countTime += Time.deltaTime;
        }

        //超過分修正
        obj.RotateAround(centerOfRotation, axis, -1 * 360f * (countTime - time) / time);

        //rotationとcameraの設定を回転前に戻して終了
        //obj.rotation = beforeRotation;→これはダメ。なぜなら他の処理でrotationが、デフォルトから変わっているかもしれないから
        if (!moveCameraInRotation) ThirdPersonCameraController.Instance.IsMoving = true;
        yield break;
    }

    /// <summary>
    /// 特定の軸でobjを1回転させる
    /// </summary>
    /// <param name="obj">回転させるGameObjectのTransform</param>
    /// <param name="axis">回転する軸</param>
    /// <param name="time">一回転するのに用する時間(秒)</param>
    /// <param name="moveCameraInRotation">回転中もカメラをプレイヤーに合わせて動かすか</param>
    public static IEnumerator OneRotationInCertainTime(Transform obj, Vector3 axis, float time, bool moveCameraInRotation)
    {
        //カメラの追従を一時停止
        if (!moveCameraInRotation) ThirdPersonCameraController.Instance.IsMoving = false;

        Vector3 centerOfRotation = obj.position;
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
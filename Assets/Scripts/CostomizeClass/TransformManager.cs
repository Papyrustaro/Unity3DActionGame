using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour
{
    public static Vector3 GetAxis(Transform _transform, E_TransformAxis transformAxis)
    {
        if (transformAxis == E_TransformAxis.Forward) return _transform.forward;
        if (transformAxis == E_TransformAxis.Up) return _transform.up;
        if (transformAxis == E_TransformAxis.Right) return _transform.right;

        throw new System.Exception();
    }

    /// <summary>
    /// 自分のgameObjectが中心にいないときの一定時間で一定角度回転処理
    /// </summary>
    /// <param name="obj">回転させるtransform</param>
    /// <param name="centerPosition">実際の回転中心transform</param>
    /// <param name="axis">回転させる軸</param>
    /// <param name="angle">回転角度</param>
    /// <param name="time">回転時間</param>
    /// <returns></returns>
    public static IEnumerator RotateInCertainTimeByAxisFromAway(Transform obj, Transform centerPosition, E_TransformAxis axis, float angle, float time)
    {
        float countTime = 0f;

        //回転処理
        while (countTime < time)
        {
            yield return null;
            countTime += Time.deltaTime;
            obj.RotateAround(centerPosition.position, GetAxis(centerPosition, axis), angle * Time.deltaTime / time);
        }

        //超過分修正
        obj.RotateAround(centerPosition.position, GetAxis(centerPosition, axis), -1 * angle * (countTime - time) / time);
        yield break;
    }

    /// <summary>
    /// 自分のgameObjectが中心にいないときの一定時間で一定角度回転処理
    /// </summary>
    /// <param name="obj">回転させるtransform</param>
    /// <param name="centerPosition">実際の回転中心transform</param>
    /// <param name="axis">回転させる軸</param>
    /// <param name="angle">回転角度</param>
    /// <param name="time">回転時間</param>
    /// <returns></returns>
    public static IEnumerator RotateInCertainTimeByAxisFromAway(Transform obj, Transform centerPosition, E_TransformAxis axis, float angle, float beginTime, float time)
    {
        yield return new WaitForSeconds(beginTime);
        float countTime = 0f;

        //回転処理
        while (countTime < time)
        {
            yield return null;
            countTime += Time.deltaTime;
            obj.RotateAround(centerPosition.position, GetAxis(centerPosition, axis), angle * Time.deltaTime / time);
        }

        //超過分修正
        obj.RotateAround(centerPosition.position, GetAxis(centerPosition, axis), -1 * angle * (countTime - time) / time);
        yield break;
    }

    /// <summary>
    /// 自分のgameObjectが中心にいないときの一定時間で一定角度回転処理
    /// </summary>
    /// <param name="obj">回転させるtransform</param>
    /// <param name="centerPosition">実際の回転中心transform</param>
    /// <param name="axis">回転させる軸</param>
    /// <param name="angle">回転角度</param>
    /// <param name="time">回転時間</param>
    /// <returns></returns>
    public static IEnumerator RotateInCertainTimeByFixedAxisFromAway(Transform obj, Transform centerPosition, E_TransformAxis axis, float angle, float time)
    {
        float countTime = 0f;
        Vector3 rotateAxis = GetAxis(centerPosition, axis);

        //回転処理
        while (countTime < time)
        {
            yield return null;
            countTime += Time.deltaTime;
            obj.RotateAround(centerPosition.position, rotateAxis, angle * Time.deltaTime / time);
        }

        //超過分修正
        obj.RotateAround(centerPosition.position, rotateAxis, -1 * angle * (countTime - time) / time);
        yield break;
    }

    /// <summary>
    /// 自分のgameObjectが中心にいるときの一定時間で一定角度回転処理
    /// </summary>
    /// <param name="obj">回転させるtransform</param>
    /// <param name="axis">回転させる軸</param>
    /// <param name="angle">回転角度</param>
    /// <param name="time">回転時間</param>
    /// <returns></returns>
    public static IEnumerator RotateInCertainTimeByAxis(Transform obj, E_TransformAxis axis, float angle, float time)
    {
        float countTime = 0f;

        //回転処理
        while (countTime < time)
        {
            yield return null;
            countTime += Time.deltaTime;
            obj.Rotate(GetAxis(obj, axis), angle * Time.deltaTime / time);
        }

        //超過分修正
        obj.Rotate(GetAxis(obj, axis), -1 * angle * (countTime - time) / time);
        yield break;
    }
}

public enum E_TransformAxis
{
    Forward,
    Up,
    Right
}

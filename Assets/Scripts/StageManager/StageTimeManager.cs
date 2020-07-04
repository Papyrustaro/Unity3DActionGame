using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ全体の一時停止などの処理
/// </summary>
public class StageTimeManager : MonoBehaviour
{
    public static StageTimeManager Instance { get; private set; }

    public bool AllStop { get; set; } = false;

    public bool StageStop { get; set; } = false;

    public bool PlayerStop { get; set; } = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }
    }

    /// <summary>
    /// 移動するステージが今移動できるかどうか
    /// </summary>
    public bool StageAbleMove => (!AllStop && !StageStop);
}

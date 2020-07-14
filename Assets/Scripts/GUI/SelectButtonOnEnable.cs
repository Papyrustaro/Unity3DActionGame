using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ボタンがenable=trueになったときに選択される
/// </summary>
public class SelectButtonOnEnable : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        this._button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        StartCoroutine(CoroutineManager.DelayMethodRealTime(1, () => this._button.Select()));
    }
}

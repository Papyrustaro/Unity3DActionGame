using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using KanKikuchi.AudioManager;

public class TitleManager : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField inputPlayerNameField;

    private void Start()
    {
        StartCoroutine(CoroutineManager.DelayMethod(1, () => this.inputPlayerNameField.Select()));
        this.inputPlayerNameField.onEndEdit.AddListener((text) => this.InputPlayerName(text));
    }
    /// <summary>
    /// プレイヤー名を入力したら、menu画面へ遷移する
    /// </summary>
    public void InputPlayerName(string inputText)
    {
        if(inputText != "")
        {
            StaticData.playerName = inputText;
            SceneManager.LoadScene("Menu");
        }
    }
}

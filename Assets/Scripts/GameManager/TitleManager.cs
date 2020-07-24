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
    [SerializeField] private InputField inputPlayerNameField;
    [SerializeField] private GameObject announceText;

    private void Start()
    {
        StartCoroutine(CoroutineManager.DelayMethod(1, () => this.inputPlayerNameField.Select()));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputPlayerName();
        }
    }

    /// <summary>
    /// プレイヤー名を入力したら、menu画面へ遷移する
    /// </summary>
    public void InputPlayerName()
    {
        if(this.inputPlayerNameField.text != "")
        {
            SEManager.Instance.Play(SEPath.DECISION1, volumeRate: 0.3f);
            StaticData.playerName = this.inputPlayerNameField.text;
            SceneManager.LoadScene("Menu");
        }
    }

    public void InputTextChanged()
    {
        this.announceText.SetActive(this.inputPlayerNameField.text != "");
    }
}

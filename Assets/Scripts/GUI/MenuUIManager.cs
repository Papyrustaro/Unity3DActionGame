﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject initMenuView;
    [SerializeField] private GameObject selectStageView;
    [SerializeField] private GameObject showRankingView;
    [SerializeField] private GameObject showManualView;
    [SerializeField] private GameObject showOptionView;
    [SerializeField] private GameObject confirmGoTutorialView;
    [SerializeField] private GameObject confirmGoTitleView;

    [SerializeField] private Text announceTextInInitMenu;

    //[SerializeField] private Text selectStageTitleText;
    [SerializeField] private Text stageInformationText;
    [SerializeField] private GameObject stage10LockIcon;
    [SerializeField] private InputField stage10KeyInputField;
    [SerializeField] private Button stage10Button;
    [SerializeField] private List<Button> selectStageViewButtons = new List<Button>();


    private E_MenuScene currentScene = E_MenuScene.InitMenu;

    public static MenuUIManager Instance { get; private set; }

    public Text StageInformationText => this.stageInformationText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception();
        }

        if (StaticData.isOpenStage10)
        {
            this.stage10KeyInputField.gameObject.SetActive(false);
            this.stage10LockIcon.SetActive(false);
        }
    }

    public void SelectMenuButton(E_MenuButton menuType)
    {
        switch (menuType)
        {
            case E_MenuButton.GoBackTitle:
                this.announceTextInInitMenu.text = "タイトル画面にもどります";
                break;
            case E_MenuButton.GoTutorial:
                this.announceTextInInitMenu.text = "【初めての方向け】\nチュートリアルを開始します";
                break;
            case E_MenuButton.SelectStage:
                this.announceTextInInitMenu.text = "ステージ選択に移ります";
                break;
            case E_MenuButton.ShowManual:
                this.announceTextInInitMenu.text = "以下について確認できます\n\n・操作方法\n・アクション\n・ステージギミック\n・制作";
                break;
            case E_MenuButton.ShowOption:
                this.announceTextInInitMenu.text = "設定を変更します";
                break;
            case E_MenuButton.ShowRanking:
                this.announceTextInInitMenu.text = "ステージ毎のタイムランキングを表示します";
                break;
        }
    }

    public void PressMenuButton(E_MenuButton menuType)
    {
        this.initMenuView.SetActive(false);
        switch (menuType)
        {
            case E_MenuButton.GoBackTitle:
                this.confirmGoTitleView.SetActive(true);
                this.currentScene = E_MenuScene.ConfirmGoBackTitle;
                break;
            case E_MenuButton.GoTutorial:
                this.confirmGoTutorialView.SetActive(true);
                this.currentScene = E_MenuScene.ConfirmGoTutorial;
                break;
            case E_MenuButton.SelectStage:
                this.selectStageView.SetActive(true);
                this.currentScene = E_MenuScene.SelectStage;
                break;
            case E_MenuButton.ShowManual:
                this.showManualView.SetActive(true);
                this.currentScene = E_MenuScene.ShowManual;
                break;
            case E_MenuButton.ShowOption:
                this.showOptionView.SetActive(true);
                this.currentScene = E_MenuScene.ShowOption;
                break;
            case E_MenuButton.ShowRanking:
                this.showRankingView.SetActive(true);
                this.currentScene = E_MenuScene.ShowRanking;
                break;
        }
    }

    public void PressBackButton()
    {
        switch (this.currentScene)
        {
            case E_MenuScene.SelectStage:
                this.selectStageView.SetActive(false);
                break;
            case E_MenuScene.ShowManual:
                this.showManualView.SetActive(false);
                break;
            case E_MenuScene.ShowOption:
                this.showOptionView.SetActive(false);
                break;
            case E_MenuScene.ShowRanking:
                this.showRankingView.SetActive(false);
                break;
            case E_MenuScene.ConfirmGoBackTitle:
                this.confirmGoTitleView.SetActive(false);
                break;
            case E_MenuScene.ConfirmGoTutorial:
                this.confirmGoTutorialView.SetActive(false);
                break;
            case E_MenuScene.InitMenu:
                break;
        }
        this.initMenuView.SetActive(true);
        this.currentScene = E_MenuScene.InitMenu;
    }

    /// <summary>
    /// ステージを選択したときの遷移処理
    /// </summary>
    /// <param name="selectStageIndex">選択したステージのindex</param>
    public void SelectStage(int selectStageIndex)
    {
        if(selectStageIndex == 9 && !StaticData.isOpenStage10)
        {
            this.stage10KeyInputField.Select();
        }
        else
        {
            foreach(Button b in this.selectStageViewButtons)
            {
                b.enabled = false;
            }
            BGMManager.Instance.Play(BGMPath.STAGE_BGM1, volumeRate: 0.5f);
            StartCoroutine(CoroutineManager.DelayMethod(0.5f, () => SceneManager.LoadScene("Stage" + (selectStageIndex + 1).ToString())));
        }
    }

    public void InputStage10Key()
    {
        if (this.stage10KeyInputField.text == "5638")
        {
            StaticData.isOpenStage10 = true;
            this.stage10LockIcon.SetActive(false);
            this.stage10KeyInputField.gameObject.SetActive(false);
            SEManager.Instance.Play(SEPath.CORRECT);
        }
        else
        {
            SEManager.Instance.Play(SEPath.INCORRECT, 0.2f);
        }
        this.stage10KeyInputField.text = "";
        this.stage10Button.Select();
    }

    /// <summary>
    /// チュートリアルを開始する
    /// </summary>
    public void GoTutorial()
    {
        StartCoroutine(CoroutineManager.DelayMethod(0.1f, () => SceneManager.LoadScene("Tutorial")));
    }

    /// <summary>
    /// タイトル画面にもどる
    /// </summary>
    public void GoTitle()
    {
        StartCoroutine(CoroutineManager.DelayMethod(0.1f, () => SceneManager.LoadScene("Title")));
    }


    public enum E_MenuScene
    {
        InitMenu,
        SelectStage,
        ConfirmGoTutorial,
        ShowRanking,
        ShowManual,
        ShowOption,
        ConfirmGoBackTitle
    }

}

public enum E_MenuButton
{
    SelectStage,
    GoTutorial,
    ShowRanking,
    ShowManual,
    ShowOption,
    GoBackTitle
}
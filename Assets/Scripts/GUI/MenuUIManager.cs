using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

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
                this.announceTextInInitMenu.text = "以下について確認できます\n\n・操作方法\n・アクション\n・ステージギミック\n・使用した素材";
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
                break;
            case E_MenuButton.ShowOption:
                break;
            case E_MenuButton.ShowRanking:
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
        
        SceneManager.LoadScene("Stage" + selectStageIndex.ToString());
    }

    /// <summary>
    /// チュートリアルを開始する
    /// </summary>
    public void GoTutorial()
    {
        SceneManager.LoadScene("Tutorial");
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
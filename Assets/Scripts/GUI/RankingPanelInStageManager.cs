using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanelInStageManager : MonoBehaviour
{
    [SerializeField] private Button showRecentClearButton;
    [SerializeField] private Button showTimeRankingButton;
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject timeRankingScrollView;
    [SerializeField] private GameObject recentClearScrollView;

    public void ShowRecentClear()
    {
        this.timeRankingScrollView.SetActive(false);
        this.recentClearScrollView.SetActive(true);
        this.showRecentClearButton.gameObject.SetActive(false);
        this.showTimeRankingButton.gameObject.SetActive(true);
        this.showTimeRankingButton.Select();
        this.titleText.text = "最近クリアした人";
    }

    public void ShowTimeRanking()
    {
        this.timeRankingScrollView.SetActive(true);
        this.recentClearScrollView.SetActive(false);
        this.showRecentClearButton.gameObject.SetActive(true);
        this.showTimeRankingButton.gameObject.SetActive(false);
        this.showRecentClearButton.Select();
        this.titleText.text = "タイムランキング";
    }

    public void PressContinueButton()
    {
        StageUIManager.Instance.SavePlayerResultBeforeMoveScene();
        StageManager.Instance.ContinueStage();
    }

    public void PressGoMenuButton()
    {
        StageUIManager.Instance.SavePlayerResultBeforeMoveScene();
        StageManager.Instance.GoMenu();
    }
}

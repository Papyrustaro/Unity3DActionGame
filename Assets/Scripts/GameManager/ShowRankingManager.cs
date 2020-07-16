using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System;

public class ShowRankingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> scrollViews = new List<GameObject>();
    [SerializeField] private List<Text> playerNamesTexts = new List<Text>();
    [SerializeField] private List<Text> clearTimesTexts = new List<Text>();
    [SerializeField] private int stageCount = 11;
    [SerializeField] private Text rankingTitleText;
    [SerializeField] private Button nextRankingIndexButton;
    private int showingStageIndex = 0;

    public Text RankingTitleText => this.rankingTitleText;
    public int ShowingStageIndex => this.showingStageIndex;
    public Button NextRankingIndexButton => this.nextRankingIndexButton;


    public void SetRanking()
    {
        for (int i = 0; i < stageCount; i++)
        {
            if (StaticData.highRankResults.ContainsKey("Stage" + (i + 1).ToString()))
            {
                this.playerNamesTexts[i].text = StaticData.highRankResults["Stage" + (i + 1).ToString()].PlayerNameText;
                this.clearTimesTexts[i].text = StaticData.highRankResults["Stage" + (i + 1).ToString()].ResultTimeText;
            }
            else
            {
                StartCoroutine(SetHighRankingTexts(i));
            }
        }
    }

    public IEnumerator SetHighRankingTexts(int stageIndex)
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Stage" + (stageIndex + 1).ToString());
        List<NCMBObject> result = null;
        NCMBException error = null;

        query.OrderByAscending("ClearTime"); //昇順
        query.Limit = 10;

        query.FindAsync((List<NCMBObject> _result, NCMBException _error) =>
        {
            result = _result;
            error = _error;
        });

        //resultもしくはerrorが入るまで待機
        yield return new WaitWhile(() => result == null && error == null);

        //後続処理
        if (error == null)
        {
            this.SetHighRankingTexts(result, stageIndex);
        }
    }

    public void SetHighRankingTexts(List<NCMBObject> highRanks, int stageIndex)
    {
        string playerName = "";
        string resultTime = "";
        List<ResultDataNameAndTime> results = new List<ResultDataNameAndTime>();
        for (int i = 0; i < highRanks.Count; i++)
        {
            playerName += (i + 1).ToString() + ". " + highRanks[i]["PlayerName"].ToString() + "\n";
            resultTime += highRanks[i]["ClearTime"].ToString() + "\n";
        }
        this.playerNamesTexts[stageIndex].text = playerName;
        this.clearTimesTexts[stageIndex].text = resultTime;
        StaticData.highRankResults.Add("Stage" + (stageIndex + 1).ToString(), new ResultDataNameAndTime(playerName, resultTime));
    }

    public void PressNextStageRankingButton()
    {
        //SEManager.PlaySE(SEManager.select);
        this.scrollViews[this.showingStageIndex].SetActive(false);
        this.showingStageIndex++;
        if (this.showingStageIndex > this.stageCount - 1) this.showingStageIndex = 0;
        this.scrollViews[this.showingStageIndex].SetActive(true);
        this.RankingTitleText.text = "ランキング(Stage" + (this.showingStageIndex + 1).ToString() + ")";
    }

    public void PressBackStageRankingButton()
    {
        //SEManager.PlaySE(SEManager.select);
        this.scrollViews[this.showingStageIndex].SetActive(false);
        this.showingStageIndex--;
        if (this.showingStageIndex < 0) this.showingStageIndex = this.stageCount - 1;
        this.scrollViews[this.showingStageIndex].SetActive(true);
        this.RankingTitleText.text = "ランキング(Stage" + (this.showingStageIndex + 1).ToString() + ")";
    }

    public void Resume()
    {
        //StageManager.Instance.IsStop = false;
        //SEManager.PlaySE(SEManager.back);
        this.gameObject.SetActive(false);
    }
}
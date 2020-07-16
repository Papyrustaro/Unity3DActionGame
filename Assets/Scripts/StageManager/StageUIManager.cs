using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;



public class StageUIManager : MonoBehaviour
{
    [SerializeField] private Text recentPlayerNameText;
    [SerializeField] private Text recentDateText;
    [SerializeField] private Text rankingPlayerNameText;
    [SerializeField] private Text rankingTimeText;

    [SerializeField] private GameObject smogPanel;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject stageClearText;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button tweetResultButton;

    private Text playerResultText;
    private int thisTimePlayerRank;
    public IEnumerator SetHighRankingTextFromClearResult()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(SceneManager.GetActiveScene().name);
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
            this.SetHighRankingTextFromClearResult(result);
        }
        else
        {
            Debug.Log(error);
        }
    }

    public IEnumerator SetRecentClearTextFromClearResult()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(SceneManager.GetActiveScene().name);
        List<NCMBObject> result = null;
        NCMBException error = null;

        query.OrderByDescending("createDate");
        query.Limit = 9;

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
            this.SetRecentClearTextFromClearResult(result);
        }
        else
        {
            Debug.Log(error);
        }
    }

    public void SetRecentClearTextFromClearResult(List<NCMBObject> recentResults)
    {
        string playerName = "1. " + StaticData.playerName + "\n";
        string clearDate = DateTime.Now.ToString("yyyy/MM/dd") + "\n";

        for (int i = 0; i < recentResults.Count; i++)
        {
            playerName += (i + 2).ToString() + ". " + recentResults[i]["PlayerName"].ToString() + "\n";
            DateTime cDate = DateTime.Parse(recentResults[i].CreateDate.ToString());
            clearDate += cDate.ToString("yyyy/MM/dd") + "\n";
        }

        this.recentPlayerNameText.text = playerName;
        this.recentDateText.text = clearDate;

        if (StaticData.recentResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            StaticData.recentResults[SceneManager.GetActiveScene().name] = new ResultDataNameAndDate(playerName, clearDate);
        }
        else
        {
            StaticData.recentResults.Add(SceneManager.GetActiveScene().name, new ResultDataNameAndDate(playerName, clearDate));
        }
    }

    public IEnumerator SetHighRankingTextFromFailedResult()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(SceneManager.GetActiveScene().name);
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
            this.SetHighRankingTextFromFailedResult(result);
        }
    }

    public IEnumerator SetRecentClearTextFromFailedResult()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(SceneManager.GetActiveScene().name);
        List<NCMBObject> result = null;
        NCMBException error = null;

        query.OrderByDescending("createDate");
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
            this.SetRecentClearTextFromFailedResult(result);
        }
    }

    public void SetRecentClearTextFromFailedResult(List<NCMBObject> recentResults)
    {
        string playerName = "";
        string clearDate = "";
        List<ResultDataNameAndDate> results = new List<ResultDataNameAndDate>();

        for (int i = 0; i < recentResults.Count; i++)
        {
            playerName += (i + 1).ToString() + ". " + recentResults[i]["PlayerName"].ToString() + "\n";
            DateTime cDate = DateTime.Parse(recentResults[i].CreateDate.ToString());
            clearDate += cDate.ToString("yyyy/MM/dd") + "\n";
        }

        this.recentPlayerNameText.text = playerName;
        this.recentDateText.text = clearDate;
        if (StaticData.recentResults.ContainsKey(SceneManager.GetActiveScene().name)) StaticData.recentResults[SceneManager.GetActiveScene().name] = new ResultDataNameAndDate(playerName, clearDate);
        else StaticData.recentResults.Add(SceneManager.GetActiveScene().name, new ResultDataNameAndDate(playerName, clearDate));
    }

    public void SetHighRankingTextFromFailedResult(List<NCMBObject> highRanks)
    {
        string playerName = "";
        string resultTime = "";
        List<ResultDataNameAndTime> results = new List<ResultDataNameAndTime>();
        for (int i = 0; i < highRanks.Count; i++)
        {
            playerName += (i + 1).ToString() + ". " + highRanks[i]["PlayerName"].ToString() + "\n";
            resultTime += highRanks[i]["ClearTime"].ToString() + "\n";
        }
        this.rankingPlayerNameText.text = playerName;
        this.rankingTimeText.text = resultTime;
        if (StaticData.highRankResults.ContainsKey(SceneManager.GetActiveScene().name)) StaticData.highRankResults[SceneManager.GetActiveScene().name] = new ResultDataNameAndTime(playerName, resultTime);
        else StaticData.highRankResults.Add(SceneManager.GetActiveScene().name, new ResultDataNameAndTime(playerName, resultTime));
    }

    public void SetHighRankingTextFromClearResult(List<NCMBObject> highRanks)
    {
        bool rankined = false;
        string playerName = "";
        string resultTime = "";
        int highRanksCount = highRanks.Count;

        List<float> resultTimes = new List<float>();
        int thisTimeIndex = highRanksCount; //今回クリアしたプレイヤーの順位
        for (int i = 0; i < highRanksCount; i++)
        {
            if (float.Parse(highRanks[i]["ClearTime"].ToString()) > StageTimeManager.Instance.CountTime)
            {
                thisTimeIndex = i;
                break;
            }
        }
        if (highRanksCount < 10) highRanksCount++;

        for (int i = 0; i < highRanksCount; i++)
        {
            if (i == thisTimeIndex) //playerの順位
            {
                playerName += (i + 1).ToString() + ". " + StaticData.playerName + "\n";
                resultTime += StageTimeManager.Instance.CountTime + "\n";
                this.thisTimePlayerRank = i + 1;
                rankined = true;
            }
            else if (rankined) //playerがランクインしたあとの
            {
                playerName += (i + 1).ToString() + ". " + highRanks[i - 1]["PlayerName"].ToString() + "\n";
                resultTime += float.Parse((highRanks[i - 1]["ClearTime"]).ToString()) + "\n";
            }
            else //playerがランクインする前
            {
                playerName += (i + 1).ToString() + ". " + highRanks[i]["PlayerName"].ToString() + "\n";
                resultTime += float.Parse((highRanks[i]["ClearTime"]).ToString()) + "\n";
            }
        }

        this.rankingPlayerNameText.text = playerName;
        this.rankingTimeText.text = resultTime;
        if (StaticData.highRankResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            StaticData.highRankResults[SceneManager.GetActiveScene().name] = new ResultDataNameAndTime(playerName, resultTime);
        }
        else
        {
            StaticData.highRankResults.Add(SceneManager.GetActiveScene().name, new ResultDataNameAndTime(playerName, resultTime));
        }
    }




    public void SetResultAndShowUsedStaticData()
    {
        this.rankingPlayerNameText.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].PlayerNameText;
        this.rankingTimeText.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].ResultTimeText;
        this.recentPlayerNameText.text = StaticData.recentResults[SceneManager.GetActiveScene().name].PlayerNameText;
        this.recentDateText.text = StaticData.recentResults[SceneManager.GetActiveScene().name].ClearDateText;

        this.playerResultText.text = StaticData.playerName + ": " + StageTimeManager.Instance.CountTime;
        StageTimeManager.Instance.AllStop = true;
        //StartCoroutine(DelayMethodRealTime(0.3f, () =>
        //{
        //this.scoreText.SetActive(false);
        //this.timeText.SetActive(false);
        //this.flagCountText.transform.gameObject.SetActive(false);
        this.gameOverText.SetActive(true);
        this.smogPanel.SetActive(true);
        //}));
        StartCoroutine(CoroutineManager.DelayMethodRealTime(0.3f, () =>
        {
            this.gameOverText.SetActive(false);
        }));
        StartCoroutine(CoroutineManager.DelayMethodRealTime(0.5f, () =>
        {
            //SEManager.PlaySE(SEManager.getItem);
            this.rankingPanel.SetActive(true);
            this.tweetResultButton.gameObject.SetActive(false);
            this.continueButton.Select();
        }));
    }

    public IEnumerator SetAndShowRankingWhenClear()
    {
        this.playerResultText.text = StaticData.playerName + ": " + StageTimeManager.Instance.CountTime;
        StageTimeManager.Instance.AllStop = true; 

        //StartCoroutine(DelayMethodRealTime(0.5f, () =>
        //{
        //SEManager.PlaySE(SEManager.success);
        //this.scoreText.SetActive(false);
        //this.timeText.SetActive(false);
        //this.flagCountText.transform.gameObject.SetActive(false);
        this.stageClearText.SetActive(true);
        this.smogPanel.SetActive(true);
        //}));
        StartCoroutine(CoroutineManager.DelayMethodRealTime(0.5f, () =>
        {
            this.stageClearText.SetActive(false);
        }));

        StartCoroutine(SetHighRankingTextFromClearResult());
        yield return StartCoroutine(SetRecentClearTextFromClearResult());

        //SEManager.PlaySE(SEManager.getItem);
        this.rankingPanel.SetActive(true);
        this.continueButton.Select();
        StageManager.Instance.SavePlayerResult();
    }

    public IEnumerator SetAndShowRankingWhenFailed()
    {
        this.playerResultText.text = StaticData.playerName + ": " + StageTimeManager.Instance.CountTime;
        StageTimeManager.Instance.AllStop = true;

        //StartCoroutine(DelayMethodRealTime(0.5f, () =>
        //{
        //SEManager.PlaySE(SEManager.failed);
        //this.scoreText.SetActive(false);
        //this.timeText.SetActive(false);
        //this.flagCountText.transform.gameObject.SetActive(false);
        this.gameOverText.SetActive(true);
        this.smogPanel.SetActive(true);
        //}));
        StartCoroutine(CoroutineManager.DelayMethodRealTime(0.5f, () =>
        {
            this.gameOverText.SetActive(false);
        }));

        if (StaticData.highRankResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            this.rankingPlayerNameText.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].PlayerNameText;
            this.rankingTimeText.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].ResultTimeText;
        }
        else
        {
            yield return StartCoroutine(SetHighRankingTextFromFailedResult());
        }

        if (StaticData.recentResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            this.recentPlayerNameText.text = StaticData.recentResults[SceneManager.GetActiveScene().name].PlayerNameText;
            this.recentDateText.text = StaticData.recentResults[SceneManager.GetActiveScene().name].ClearDateText;
        }
        else
        {
            yield return StartCoroutine(SetRecentClearTextFromFailedResult());
        }

        //SEManager.PlaySE(SEManager.getItem);
        this.rankingPanel.SetActive(true);
        this.tweetResultButton.gameObject.SetActive(false);
        this.continueButton.Select();
    }
}

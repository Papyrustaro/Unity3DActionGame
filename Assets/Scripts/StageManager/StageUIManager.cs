using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using KanKikuchi.AudioManager;



public class StageUIManager : MonoBehaviour
{   
    [SerializeField] private GameObject gameClearText;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Text recentPlayerNameTextInClear;
    [SerializeField] private Text recentDateTextInClear;
    [SerializeField] private Text rankingPlayerNameTextInClear;
    [SerializeField] private Text rankingTimeTextInClear;
    [SerializeField] private Text playerResultText;

    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text recentPlayerNameTextInFailed;
    [SerializeField] private Text recentDateTextInFailed;
    [SerializeField] private Text rankingPlayerNameTextInFailed;
    [SerializeField] private Text rankingTimeTextInFailed;

    private bool sendResultThisTime = false;

    private int thisTimePlayerRank = -1;

    private bool isClear = false;

    public static StageUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else throw new Exception();
    }

    public void GameOver()
    {
        SEManager.Instance.Play(SEPath.FAILED, 0.5f);
        StageTimeManager.Instance.CountTimeStop = true;
        StageTimeManager.Instance.SetActiveCountTime(false);
        StageCameraManager.Instance.SetAbleFollow(false);
        StartCoroutine(this.SetAndShowRankingWhenFailed());
    }

    public void GameClear()
    {
        SEManager.Instance.Play(SEPath.SUCCESS, 0.5f);
        this.isClear = true;
        StageTimeManager.Instance.CountTimeStop = true;
        StageTimeManager.Instance.SetActiveCountTime(false);
        StageCameraManager.Instance.SetAbleFollow(false);
        StartCoroutine(this.SetAndShowRankingWhenClear());
        StartCoroutine(CoroutineManager.DelayMethod(5f, () => SavePlayerResult()));
    }

    public void SavePlayerResult()
    {
        if (this.sendResultThisTime) return;
        this.sendResultThisTime = true;
        NCMBObject obj = new NCMBObject(SceneManager.GetActiveScene().name);
        obj["PlayerName"] = StaticData.playerName;
        obj["ClearTime"] = StageTimeManager.Instance.CountTime;
        obj.SaveAsync();
    }

    public void SavePlayerResultBeforeMoveScene()
    {
        if(this.isClear)
        {
            this.SavePlayerResult();
        }
    }

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
        string playerName = "1." + StaticData.playerName + "\n";
        string clearDate = DateTime.Now.ToString("yyyy/MM/dd") + "\n";

        for (int i = 0; i < recentResults.Count; i++)
        {
            playerName += (i + 2).ToString() + "." + recentResults[i]["PlayerName"].ToString() + "\n";
            DateTime cDate = DateTime.Parse(recentResults[i].CreateDate.ToString());
            clearDate += cDate.ToString("yyyy/MM/dd") + "\n";
        }

        this.recentPlayerNameTextInClear.text = playerName;
        this.recentDateTextInClear.text = clearDate;

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
            playerName += (i + 1).ToString() + "." + recentResults[i]["PlayerName"].ToString() + "\n";
            DateTime cDate = DateTime.Parse(recentResults[i].CreateDate.ToString());
            clearDate += cDate.ToString("yyyy/MM/dd") + "\n";
        }

        this.recentPlayerNameTextInFailed.text = playerName;
        this.recentDateTextInFailed.text = clearDate;
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
            playerName += (i + 1).ToString() + "." + highRanks[i]["PlayerName"].ToString() + "\n";
            resultTime += highRanks[i]["ClearTime"].ToString() + "\n";
        }
        this.rankingPlayerNameTextInFailed.text = playerName;
        this.rankingTimeTextInFailed.text = resultTime;
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
                playerName += (i + 1).ToString() + "." + StaticData.playerName + "\n";
                resultTime += StageTimeManager.Instance.CountTime + "\n";
                this.thisTimePlayerRank = i + 1;
                rankined = true;
            }
            else if (rankined) //playerがランクインしたあとの
            {
                playerName += (i + 1).ToString() + "." + highRanks[i - 1]["PlayerName"].ToString() + "\n";
                resultTime += float.Parse((highRanks[i - 1]["ClearTime"]).ToString()) + "\n";
            }
            else //playerがランクインする前
            {
                playerName += (i + 1).ToString() + "." + highRanks[i]["PlayerName"].ToString() + "\n";
                resultTime += float.Parse((highRanks[i]["ClearTime"]).ToString()) + "\n";
            }
        }

        this.rankingPlayerNameTextInClear.text = playerName;
        this.rankingTimeTextInClear.text = resultTime;
        if (StaticData.highRankResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            StaticData.highRankResults[SceneManager.GetActiveScene().name] = new ResultDataNameAndTime(playerName, resultTime);
        }
        else
        {
            StaticData.highRankResults.Add(SceneManager.GetActiveScene().name, new ResultDataNameAndTime(playerName, resultTime));
        }

        if (rankined) SEManager.Instance.Play(SEPath.RANKIN_VOICE0, volumeRate: 0.9f);
    }

    public void SetResultAndShowUsedStaticData()
    {
        /*this.rankingPlayerNameText.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].PlayerNameText;
        this.rankingTimeText.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].ResultTimeText;
        this.recentPlayerNameText.text = StaticData.recentResults[SceneManager.GetActiveScene().name].PlayerNameText;
        this.recentDateText.text = StaticData.recentResults[SceneManager.GetActiveScene().name].ClearDateText;*/

        this.playerResultText.text = StaticData.playerName + ": " + StageTimeManager.Instance.CountTime  + "秒";
        StageTimeManager.Instance.AllStop = true;
        this.gameOverText.SetActive(true);
        StartCoroutine(CoroutineManager.DelayMethodRealTime(0.3f, () =>
        {
            this.gameOverText.SetActive(false);
        }));
    }

    /// <summary>
    /// クリアしたときの描画処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator SetAndShowRankingWhenClear()
    {
        this.playerResultText.text = StaticData.playerName + ": " + StageTimeManager.Instance.CountTime + "秒";

        this.gameClearText.SetActive(true);
        
        ///ランキング情報の代入処理
        StartCoroutine(SetHighRankingTextFromClearResult());
        StartCoroutine(SetRecentClearTextFromClearResult());

        //SavePlayerResult();

        yield return new WaitForSeconds(1f);
        this.gameClearText.SetActive(false);
        this.gameClearPanel.SetActive(true);
        StageTimeManager.Instance.PlayerStop = true;
    }

    /// <summary>
    /// ゲームオーバーになったときの描画処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator SetAndShowRankingWhenFailed()
    {
        this.gameOverText.SetActive(true);

        //タイムランキング情報を取得
        if (StaticData.highRankResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            this.rankingPlayerNameTextInFailed.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].PlayerNameText;
            this.rankingTimeTextInFailed.text = StaticData.highRankResults[SceneManager.GetActiveScene().name].ResultTimeText;
        }
        else
        {
            StartCoroutine(SetHighRankingTextFromFailedResult());
        }

        //最近クリアした人を取得
        if (StaticData.recentResults.ContainsKey(SceneManager.GetActiveScene().name))
        {
            this.recentPlayerNameTextInFailed.text = StaticData.recentResults[SceneManager.GetActiveScene().name].PlayerNameText;
            this.recentDateTextInFailed.text = StaticData.recentResults[SceneManager.GetActiveScene().name].ClearDateText;
        }
        else
        {
            StartCoroutine(SetRecentClearTextFromFailedResult());
        }

        yield return new WaitForSeconds(1f);

        this.gameOverText.SetActive(false);
        this.gameOverPanel.SetActive(true);
        StageTimeManager.Instance.PlayerStop = true;
    }



    public void Tweeting()
    {
        string tweetText = "";
        if (this.thisTimePlayerRank != -1) tweetText = "【現在" + this.thisTimePlayerRank.ToString() + "位】";
        tweetText += SceneManager.GetActiveScene().name + "を" + StageTimeManager.Instance.CountTime.ToString() + "秒でクリア!!";

        string url = "https://twitter.com/intent/tweet?"
            + "text=" + tweetText
            + "&url=" + "https://unityroom.com/games/spacejumpgame"
            + "&hashtags=" + "SpaceJumpGame,unityroom";

#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
            // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
            Application.OpenURL(url);
#endif
    }


}

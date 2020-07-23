using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static string playerName = "player";

    public static bool showCountTime = true;

    public static bool isOpenStage10 = false;

    /// <summary>
    /// missionFailedのときは、こちらにランキングデータを格納し、次回以降のmissionFailed時はこちらを参照。keyはステージ名(Stage0など)。valueは上位10名のデータ
    /// </summary>
    public static Dictionary<string, ResultDataNameAndTime> highRankResults = new Dictionary<string, ResultDataNameAndTime>();

    /// <summary>
    /// missionFailedのときは、こちらにランキングデータを格納し、次回以降のmissionFailed時はこちらを参照。keyはステージ名(Stage0など)。valueは上位10名のデータ
    /// </summary>
    public static Dictionary<string, ResultDataNameAndDate> recentResults = new Dictionary<string, ResultDataNameAndDate>();
}

public class ResultDataNameAndTime
{
    public string PlayerNameText { get; private set; }
    public string ResultTimeText { get; private set; }

    public ResultDataNameAndTime(string playerNameText, string resultTimeText)
    {
        this.PlayerNameText = playerNameText; this.ResultTimeText = resultTimeText;
    }
}

public class ResultDataNameAndDate
{
    public string PlayerNameText { get; private set; }
    public string ClearDateText { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rank">日時の新しさ順位(1位は1)</param>
    /// <param name="playerName">プレイヤー名</param>
    /// <param name="clearDate">クリアした日付う</param>
    public ResultDataNameAndDate(string playerNameText, string clearDateText)
    {
        this.PlayerNameText = playerNameText; this.ClearDateText = clearDateText;
    }
}
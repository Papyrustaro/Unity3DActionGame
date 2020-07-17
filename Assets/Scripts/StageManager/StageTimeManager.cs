using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ全体の一時停止などの処理
/// </summary>
public class StageTimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countTimeText;
    [SerializeField] private GameObject timeIcon;

    private bool currentSceneIsStage = true;
    public static StageTimeManager Instance { get; private set; }

    public bool AllStop { get; set; } = false;

    [field: SerializeField]
    [field: RenameField("StageStop")]
    public bool StageStop { get; set; } = false;

    public bool PlayerStop { get; set; } = false;

    public float CountTime { get; private set; } = 0f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }
        
        string currentScene = SceneManager.GetActiveScene().name;
        this.currentSceneIsStage = currentScene != "Title" && currentScene != "Menu" && currentScene != "StageSelect";
        if (StaticData.showCountTime && this.currentSceneIsStage)
        {
            this.countTimeText.gameObject.SetActive(true);
            this.timeIcon.SetActive(true);
        }
        else
        {
            this.countTimeText.gameObject.SetActive(false);
            this.timeIcon.SetActive(false);
        }
    }

    private void Update()
    {
        if (!this.currentSceneIsStage) return;
        if (!this.AllStop) this.CountTime += Time.deltaTime;

        if (StaticData.showCountTime) this.countTimeText.text = ((int)this.CountTime).ToString();
    }

    public void SetActiveCountTime(bool flag)
    {
        this.countTimeText.gameObject.SetActive(flag);
        this.timeIcon.SetActive(flag);
    }

    /// <summary>
    /// 移動するステージが今移動できるかどうか
    /// </summary>
    public bool IsStageMoving => (!AllStop && !StageStop);

    public bool IsPlayerMoving => (!AllStop && !PlayerStop);
}

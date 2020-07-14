using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ステージ全体の一時停止などの処理
/// </summary>
public class StageTimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countTimeText;
    [SerializeField] private GameObject timeIcon;
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
        if (StaticData.showCountTime)
        {
            this.countTimeText.gameObject.SetActive(true);
            this.timeIcon.SetActive(true);
        }
    }

    private void Update()
    {
        if (!this.AllStop) this.CountTime += Time.deltaTime;
        if (StaticData.showCountTime) this.countTimeText.text = ((int)this.CountTime).ToString();
    }

    /// <summary>
    /// 移動するステージが今移動できるかどうか
    /// </summary>
    public bool IsStageMoving => (!AllStop && !StageStop);

    public bool IsPlayerMoving => (!AllStop && !PlayerStop);
}

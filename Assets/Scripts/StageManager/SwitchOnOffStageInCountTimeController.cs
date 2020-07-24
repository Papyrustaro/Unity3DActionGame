using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using KanKikuchi.AudioManager;
using UnityEngine.SceneManagement;

/// <summary>
/// 時間経過でonOffStageのフラグが変わる。
/// </summary>
public class SwitchOnOffStageInCountTimeController : MonoBehaviour
{
    [SerializeField] private float switchInterval = 3f;
    [SerializeField] private Material initOnInOnSwitchStageMaterial;
    [SerializeField] private Material initOnInOffSwitchStageMaterial;
    [SerializeField] private Material initOffInOnSwitchStageMaterial;
    [SerializeField] private Material initOffInOffSwitchStageMaterial;
    [SerializeField] private UnityEvent onSwitch;
    [SerializeField, ReadOnly] private List<SwitchOnOffStage> switchStages;
    [SerializeField] WallKickTrigger playerWallKickTrigger;
    private float countTime = 0f;
    private bool playSE = false;
    private bool isStageScene = true;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Title" || SceneManager.GetActiveScene().name == "Menu") this.isStageScene = false;
    }

    private void Update()
    {
        if (StageTimeManager.Instance.IsStageMoving)
        {
            this.countTime += Time.deltaTime;
            if(this.countTime >= this.switchInterval)
            {
                this.onSwitch.Invoke();
                if (this.playSE)
                {
                    SEManager.Instance.Play(SEPath.SWITCH_STAGE1, volumeRate: 0.2f);
                    SEManager.Instance.Play(SEPath.SWITCH_STAGE2, volumeRate: 0.2f);
                }
                foreach (SwitchOnOffStage switchStage in this.switchStages) switchStage.SwitchOnOff();
                this.countTime = 0f;
                if (this.isStageScene && this.playerWallKickTrigger.IsStickingOnDisappearableWall)
                {
                    this.playerWallKickTrigger.AwayFromOneWall();
                    this.playerWallKickTrigger.IsStickingOnDisappearableWall = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.playSE = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.playSE = false;
        }
    }

    /// <summary>
    /// シーンに存在するonOffStageを代入・初期化。Inspector上から
    /// </summary>
    [Button(enabledMode: EButtonEnableMode.Editor)]
    private void SetSwitchOnOffStages()
    {
        this.switchStages = new List<SwitchOnOffStage>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SwitchOnOffStage"))
        {
            this.switchStages.Add(obj.GetComponent<SwitchOnOffStage>());
        }
        foreach (SwitchOnOffStage switchStage in this.switchStages)
        {
            switchStage.InitSet(this.initOnInOnSwitchStageMaterial, this.initOnInOffSwitchStageMaterial, this.initOffInOnSwitchStageMaterial, this.initOffInOffSwitchStageMaterial);
        }
        this.playerWallKickTrigger = GameObject.FindGameObjectsWithTag("Player")[0].transform.Find("WallCheck").GetComponent<WallKickTrigger>();
    }
}

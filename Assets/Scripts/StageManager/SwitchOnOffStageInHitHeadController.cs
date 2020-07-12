using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using KanKikuchi.AudioManager;


public class SwitchOnOffStageInHitHeadController : MonoBehaviour
{
    [SerializeField] private Material switchToOffMaterial;
    [SerializeField] private Material switchToOnMaterial;
    [SerializeField] private UnityEvent onSwitch;
    [SerializeField] private Material initOnInOnSwitchStageMaterial;
    [SerializeField] private Material initOnInOffSwitchStageMaterial;
    [SerializeField] private Material initOffInOnSwitchStageMaterial;
    [SerializeField] private Material initOffInOffSwitchStageMaterial;
    [SerializeField, ReadOnly] private List<SwitchOnOffStageInHitHead> switches;
    [SerializeField, ReadOnly] private List<SwitchOnOffStage> switchStages;


    private bool isOn = true;
    public static SwitchOnOffStageInHitHeadController Instance { get; private set; }

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
    }

    public void SwitchAll()
    {
        SEManager.Instance.Play(SEPath.SWITCH_STAGE1, volumeRate: 0.5f);
        SEManager.Instance.Play(SEPath.SWITCH_STAGE2, volumeRate: 0.5f);
        if (this.isOn)
        {
            foreach(SwitchOnOffStageInHitHead switchInHitHead in this.switches)
            {
                switchInHitHead.Switch(this.switchToOnMaterial);
            }
        }
        else
        {
            foreach (SwitchOnOffStageInHitHead switchInHitHead in this.switches)
            {
                switchInHitHead.Switch(this.switchToOffMaterial);
            }
        }
        this.isOn = !this.isOn;
        this.onSwitch.Invoke();
        foreach (SwitchOnOffStage switchStage in this.switchStages) switchStage.SwitchOnOff();
    }

    /// <summary>
    /// シーンに存在するswitch、onOffStageを代入・初期化。Inspector上から
    /// </summary>
    [Button(enabledMode: EButtonEnableMode.Editor)]
    private void SetAllSwitchesAndOnOffStages()
    {
        this.switches = new List<SwitchOnOffStageInHitHead>();
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("SwitchOnOff"))
        {
            this.switches.Add(obj.GetComponent<SwitchOnOffStageInHitHead>());
        }

        this.switchStages = new List<SwitchOnOffStage>();
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("SwitchOnOffStage"))
        {
            this.switchStages.Add(obj.GetComponent<SwitchOnOffStage>());
        }
        foreach(SwitchOnOffStage switchStage in this.switchStages)
        {
            switchStage.InitSet(this.initOnInOnSwitchStageMaterial, this.initOnInOffSwitchStageMaterial, this.initOffInOnSwitchStageMaterial, this.initOffInOffSwitchStageMaterial);
        }
    }
}

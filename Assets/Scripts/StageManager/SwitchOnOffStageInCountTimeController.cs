using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 時間経過でonOffStageのフラグが変わる
/// </summary>
public class SwitchOnOffStageInCountTimeController : MonoBehaviour
{
    [SerializeField] private List<SwitchOnOffStage> switchStages;
    [SerializeField] private UnityEvent onSwitch;
    [SerializeField] private float switchInterval = 3f;
    private float countTime = 0f;

    private void Update()
    {
        if (StageTimeManager.Instance.IsStageMoving)
        {
            this.countTime += Time.deltaTime;
            if(this.countTime >= this.switchInterval)
            {
                Debug.Log("切り替わった");
                this.onSwitch.Invoke();
                foreach (SwitchOnOffStage switchStage in this.switchStages) switchStage.SwitchOnOff();
                this.countTime = 0f;
            }
        }
    }

    /// <summary>
    /// シーンに存在するonOffStageを代入・初期化。Inspector上から
    /// </summary>
    [ContextMenu("SetSwitchOnOffStages")]
    private void SetSwitchOnOffStages()
    {
        this.switchStages = new List<SwitchOnOffStage>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SwitchOnOffStage"))
        {
            this.switchStages.Add(obj.GetComponent<SwitchOnOffStage>());
        }
        foreach (SwitchOnOffStage switchStage in this.switchStages)
        {
            switchStage.InitSet();
        }
    }
}

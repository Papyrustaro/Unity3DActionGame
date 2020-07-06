using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SwitchOnOffStageInHitHeadController : MonoBehaviour
{
    [SerializeField] private List<SwitchOnOffStage> switchStages;
    [SerializeField] private UnityEvent onSwitch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitHeadCheck"))
        {
            Debug.Log("切り替わった");
            this.onSwitch.Invoke();
            foreach (SwitchOnOffStage switchStage in this.switchStages) switchStage.SwitchOnOff();
        }
    }

    /// <summary>
    /// シーンに存在するonOffStageを代入・初期化。Inspector上から
    /// </summary>
    [ContextMenu("SetSwitchOnOffStages")]
    private void SetSwitchOnOffStages()
    {
        this.switchStages = new List<SwitchOnOffStage>();
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("SwitchOnOffStage"))
        {
            this.switchStages.Add(obj.GetComponent<SwitchOnOffStage>());
        }
        foreach(SwitchOnOffStage switchStage in this.switchStages)
        {
            switchStage.InitSet();
        }
    }
}

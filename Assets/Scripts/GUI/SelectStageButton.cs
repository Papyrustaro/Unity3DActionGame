using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// ステージ選択画面のボタン
/// </summary>
public class SelectStageButton : MonoBehaviour
{
    [SerializeField] private int stageIndex = 0;
    [SerializeField] private string stageInformationText;

    private void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener((eventData) => MenuUIManager.Instance.StageInformationText.text = this.stageInformationText);
        eventTrigger.triggers.Add(entry);

        GetComponent<Button>().onClick.AddListener(() => MenuUIManager.Instance.SelectStage(this.stageIndex));
    }
}

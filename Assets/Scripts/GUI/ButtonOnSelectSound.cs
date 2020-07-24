using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.EventSystems;

public class ButtonOnSelectSound : MonoBehaviour
{
    private void Awake()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.SELECT, volumeRate: 0.5f));
        eventTrigger.triggers.Add(entry);
    }
}

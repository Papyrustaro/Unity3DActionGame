using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.EventSystems;

public class ButtonOnSubmitSound : MonoBehaviour
{
    [SerializeField] private E_SubmitSE seType = E_SubmitSE.Decision;
    private void Awake()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Submit;
        entry.callback = new EventTrigger.TriggerEvent();
        switch (this.seType)
        {
            case E_SubmitSE.Decision:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.DECISION, volumeRate: 0.2f));
                break;
            case E_SubmitSE.Back:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.BACK, volumeRate: 0.3f));
                break;
            case E_SubmitSE.Page:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.PAGE, volumeRate: 0.3f));
                break;
            case E_SubmitSE.Go:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.DECISION1, volumeRate: 0.3f));
                break;

        }
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback = new EventTrigger.TriggerEvent();
        switch (this.seType)
        {
            case E_SubmitSE.Decision:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.DECISION, volumeRate: 0.2f));
                break;
            case E_SubmitSE.Back:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.BACK, volumeRate: 0.3f));
                break;
            case E_SubmitSE.Page:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.PAGE, volumeRate: 0.3f));
                break;
            case E_SubmitSE.Go:
                entry.callback.AddListener((eventData) => SEManager.Instance.Play(SEPath.DECISION1, volumeRate: 0.3f));
                break;
        }
        eventTrigger.triggers.Add(entry);
    }

    public enum E_SubmitSE
    {
        Decision,
        Back,
        Page,
        Go
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonChangeImageAlphaNotSelecting : MonoBehaviour
{
    [SerializeField] private float alphaValueInNotSelecting = 0.5f;
    private Button _button;
    private Color colorInSelecting;
    private Color colorInNotSelecting;


    private void Awake()
    {
        this._button = GetComponent<Button>();
        this.colorInSelecting = this._button.image.color;
        this.colorInNotSelecting = this.colorInSelecting;
        this.colorInNotSelecting.a = this.alphaValueInNotSelecting;

        InitSet();
    }

    private void OnEnable()
    {
        this._button.image.color = this.colorInNotSelecting;
    }

    private void InitSet()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener((eventData) => this.OnSelect());
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Deselect;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener((eventData) => this.OnDeselect());
        eventTrigger.triggers.Add(entry);
    }

    private void OnSelect()
    {
        this._button.image.color = this.colorInSelecting;
    }

    private void OnDeselect()
    {
        this._button.image.color = this.colorInNotSelecting;
    }
}

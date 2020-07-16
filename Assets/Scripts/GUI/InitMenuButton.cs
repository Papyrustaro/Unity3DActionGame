using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InitMenuButton : MonoBehaviour
{
    [SerializeField] E_MenuButton buttonType;

    private void Start()
    {
        /*EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener((eventData) => MenuUIManager.Instance.SelectMenuButton(this.buttonType));
        eventTrigger.triggers.Add(entry);*/

        GetComponent<Button>().onClick.AddListener(() => MenuUIManager.Instance.PressMenuButton(this.buttonType));
    }


    public void ChangeAnnounceTextOnSelect()
    {
        MenuUIManager.Instance.SelectMenuButton(this.buttonType);
    }
}

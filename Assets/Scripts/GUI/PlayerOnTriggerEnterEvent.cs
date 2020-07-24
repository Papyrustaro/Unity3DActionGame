using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOnTriggerEnterEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnterEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.onTriggerEnterEvent.Invoke();
        }
    }
}

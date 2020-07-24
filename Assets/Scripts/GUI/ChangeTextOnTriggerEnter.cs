using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Text changeText;
    [SerializeField] private string afterText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.changeText.text = this.afterText;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextManager : MonoBehaviour
{
    [SerializeField] private Text[] debugTexts;

    public static DebugTextManager Instance { get; set; }

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

    public void ShowText(string showText, int textObjIndex)
    {
        this.debugTexts[textObjIndex].text = showText;
    }
}

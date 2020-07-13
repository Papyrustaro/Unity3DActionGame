using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static StageManager Instance { get; private set; }

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

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            this.OnPressPause();
        }
    }

    private void OnPressPause()
    {
        StageTimeManager.Instance.AllStop = !StageTimeManager.Instance.AllStop;
        this.pausePanel.SetActive(!this.pausePanel.activeSelf);
    }
}

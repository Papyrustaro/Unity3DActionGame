using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.SceneManagement;

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
        StageCameraManager.Instance.SetAbleRotateByInput(this.pausePanel.activeSelf);
        this.pausePanel.SetActive(!this.pausePanel.activeSelf);
    }

    public void StageClear()
    {
        StageUIManager.Instance.GameClear();
    }

    public void StageFailed()
    {
        StageUIManager.Instance.GameOver();
    }

    public void ContinueStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

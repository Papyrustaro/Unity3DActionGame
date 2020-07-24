using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public bool AblePause { get; set; } = true;
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
        if (this.AblePause && Input.GetButtonDown("Pause"))
        {
            this.OnPressPause();
        }
    }

    public void OnPressPause()
    {
        StageTimeManager.Instance.AllStop = !StageTimeManager.Instance.AllStop;
        StageCameraManager.Instance.SetAbleRotateByInput(this.pausePanel.activeSelf);
        this.pausePanel.SetActive(!this.pausePanel.activeSelf);
    }

    public void StageClear()
    {
        this.AblePause = false;
        StageUIManager.Instance.GameClear();
    }

    public void StageFailed()
    {
        this.AblePause = false;
        StageUIManager.Instance.GameOver();
    }

    public void ContinueStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMenu()
    {
        BGMManager.Instance.Play(BGMPath.MENU_BGM1, volumeRate: 0.3f);
        SceneManager.LoadScene("Menu");
    }
}

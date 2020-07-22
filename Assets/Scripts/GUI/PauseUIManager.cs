using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;
public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private GameObject initPauseView;
    [SerializeField] private GameObject showManualView;
    [SerializeField] private GameObject showOptionView;

    public void ContinueSameState()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowManualView()
    {
        this.initPauseView.SetActive(false);
        this.showManualView.SetActive(true);
    }

    public void ShowOptionView()
    {
        this.initPauseView.SetActive(false);
        this.showOptionView.SetActive(true);
    }

    public void ShowInitPauseView()
    {
        this.showManualView.SetActive(false);
        this.showOptionView.SetActive(false);
        this.initPauseView.SetActive(true);
    }

    public void ResumePlayStage()
    {
        StageTimeManager.Instance.AllStop = false;
        this.gameObject.SetActive(false);
    }

    public void GoBackMenu()
    {
        BGMManager.Instance.Play(BGMPath.MENU_BGM1, volumeRate: 0.3f);
        SceneManager.LoadScene("Menu");
    }
}

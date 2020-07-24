using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject explainActionView;
    [SerializeField] GameObject explainStageView;
    [SerializeField] GameObject explainInputView;

    public static TutorialManager Instance { get; private set; }

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

    private void Start()
    {
        ShowExplainInputView();
    }

    public void ShowExplainActionView()
    {
        StageManager.Instance.AblePause = false;
        this.tutorialPanel.SetActive(true);
        this.explainActionView.SetActive(true);
        StageTimeManager.Instance.AllStop = true;
        StageCameraManager.Instance.SetAbleRotateByInput(false);
    }

    public void ShowExplainStageView()
    {
        StageManager.Instance.AblePause = false;
        this.tutorialPanel.SetActive(true);
        this.explainStageView.SetActive(true);
        StageTimeManager.Instance.AllStop = true;
        StageCameraManager.Instance.SetAbleRotateByInput(false);
    }

    public void ShowExplainInputView()
    {
        StageManager.Instance.AblePause = false;
        this.tutorialPanel.SetActive(true);
        this.explainInputView.SetActive(true);
        StageTimeManager.Instance.AllStop = true;
        StageCameraManager.Instance.SetAbleRotateByInput(false);
    }

    public void ExitExplainActionView()
    {
        StageManager.Instance.AblePause = true;
        this.tutorialPanel.SetActive(false);
        this.explainActionView.SetActive(false);
        StageTimeManager.Instance.AllStop = false;
        StageCameraManager.Instance.SetAbleRotateByInput(true);
    }

    public void ExitExplainStageView()
    {
        StageManager.Instance.AblePause = true;
        this.tutorialPanel.SetActive(false);
        this.explainStageView.SetActive(false);
        StageTimeManager.Instance.AllStop = false;
        StageCameraManager.Instance.SetAbleRotateByInput(true);
    }

    public void ExitExplainInputView()
    {
        StageManager.Instance.AblePause = true;
        this.tutorialPanel.SetActive(false);
        this.explainInputView.SetActive(false);
        StageTimeManager.Instance.AllStop = false;
        StageCameraManager.Instance.SetAbleRotateByInput(true);
    }
}

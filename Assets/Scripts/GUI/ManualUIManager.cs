using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManualUIManager : MonoBehaviour
{
    [SerializeField] private GameObject initManualView;
    [SerializeField] private GameObject explainActionView;
    [SerializeField] private GameObject explainStageView;
    [SerializeField] private GameObject explainInputView;
    [SerializeField] private GameObject explainCopyrightView;

    public void GoExplainActionView()
    {
        this.initManualView.SetActive(false);
        this.explainActionView.SetActive(true);
    }

    public void GoExplainStageView()
    {
        this.initManualView.SetActive(false);
        this.explainStageView.SetActive(true);
    }

    public void GoExplainInputView()
    {
        this.initManualView.SetActive(false);
        this.explainInputView.SetActive(true);
    }

    public void GoExplainCopyrightView()
    {
        this.initManualView.SetActive(false);
        this.explainCopyrightView.SetActive(true);
    }

    public void BackToInitManualView()
    {
        this.explainActionView.SetActive(false);
        this.explainCopyrightView.SetActive(false);
        this.explainStageView.SetActive(false);
        this.explainInputView.SetActive(false);
        this.initManualView.SetActive(true);
    }
}

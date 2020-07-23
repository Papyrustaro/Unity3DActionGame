using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PauseUIManager pauseManager;

    private void Start()
    {
        StartCoroutine(CoroutineManager.DelayMethod(1, () =>
        {
            StageManager.Instance.OnPressPause();
            this.pauseManager.ShowManualView();
        }));
    }
}

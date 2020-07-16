using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class FirstInit : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        BGMManager.Instance.ChangeBaseVolume(0.5f);
        SEManager.Instance.ChangeBaseVolume(0.5f);
        SceneManager.LoadScene("Title");
    }
}

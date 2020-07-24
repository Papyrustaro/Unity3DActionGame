using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KanKikuchi.AudioManager;


public class OptionUIManager : MonoBehaviour
{
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private InputField bgmVolumeInputField;
    [SerializeField] private Slider seVolumeSlider;
    [SerializeField] private InputField seVolumeInputField;
    [SerializeField] private Toggle showTimeFlag;
    [SerializeField] private Toggle invertCameraRotationAxisY;
    [SerializeField] private Toggle invertCameraRotationHorizontal;

    private void OnEnable()
    {
        this.showTimeFlag.isOn = StaticData.showCountTime;
        this.invertCameraRotationAxisY.isOn = StaticData.invertCameraRotationAxisY;
        this.invertCameraRotationHorizontal.isOn = StaticData.invertCameraRotationHorizontal;
    }

    private void Start()
    {
        this.bgmVolumeSlider.onValueChanged.AddListener((x) =>
        {
            BGMManager.Instance.ChangeBaseVolume(x);
            this.bgmVolumeInputField.text = ((int)(x * 100)).ToString();
        });
        this.seVolumeSlider.onValueChanged.AddListener((x) =>
        {
            SEManager.Instance.ChangeBaseVolume(x);
            this.seVolumeInputField.text = ((int)(x * 100)).ToString();
        });
        this.bgmVolumeInputField.onEndEdit.AddListener((s) =>
        {
            int volume;
            try
            {
                volume = int.Parse(s);
            }
            catch (System.Exception)
            {
                return;
            }
            if (0 <= volume && volume <= 100)
            {
                this.bgmVolumeSlider.value = (float)volume / 100f;
                BGMManager.Instance.ChangeBaseVolume((float)volume / 100f);
            }
        });
        this.seVolumeInputField.onEndEdit.AddListener((s) =>
        {
            int volume;
            try
            {
                volume = int.Parse(s);
            }
            catch (System.Exception)
            {
                return;
            }
            if (0 <= volume && volume <= 100)
            {
                this.seVolumeSlider.value = (float)volume / 100f;
                SEManager.Instance.ChangeBaseVolume((float)volume / 100f);
            }
        });

        this.showTimeFlag.onValueChanged.AddListener((x) =>
        {
            StaticData.showCountTime = x;
            if (StageTimeManager.Instance != null) StageTimeManager.Instance.SetActiveCountTime(x);
        });

        this.invertCameraRotationAxisY.onValueChanged.AddListener((x) => {
            StaticData.invertCameraRotationAxisY = x;
        });

        this.invertCameraRotationHorizontal.onValueChanged.AddListener((x) =>
        {
            StaticData.invertCameraRotationHorizontal = x;
        });
    }
}

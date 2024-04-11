using ScriptableObjects.Settings;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class BrightnessSettings : MonoBehaviour
    {
        [SerializeField] private SettingsAsset defaultSettings;
        private const string BrightnessKey = "Brightness";
        private Slider _brightnessSlider;
        public event Action<float> OnBrightnessChanged; 

        private void Awake()
        {
            OnBrightnessChanged = null;
            if((_brightnessSlider = GetComponentInChildren<Slider>()) is not null)
            {
                _brightnessSlider.value = PlayerPrefs.GetFloat(BrightnessKey);
                _brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
            }
        }

        private void Start()
        {
            InitBrightnessPrefs();
            ChangeBrightness(_brightnessSlider.value);
        }

        private void InitBrightnessPrefs()
        {
            if(!PlayerPrefs.HasKey(BrightnessKey)) PlayerPrefs.SetFloat(BrightnessKey, defaultSettings.brightnessLevel);
            PlayerPrefs.Save();
        }


        public void ChangeBrightness(float brightnessValue)
        {
            var actualBrightness = Mathf.Lerp(0.1f, 1f, brightnessValue);
            OnBrightnessChanged?.Invoke(actualBrightness);
            PlayerPrefs.SetFloat(BrightnessKey, actualBrightness);
        }

        private void OnDestroy()
        {
            OnBrightnessChanged = null;
            PlayerPrefs.Save();
        }
    }
}
using System;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class BrightnessSettings : MonoBehaviour
    {
        private const string BrightnessKey = "Brightness";
        private Slider _brightnessSlider;
        public event Action<float> OnBrightnessChanged; 
        [SerializeField] private SettingsAsset defaultSettings;

        private void Awake()
        {
            if(!PlayerPrefs.HasKey(BrightnessKey)) PlayerPrefs.SetFloat(BrightnessKey, defaultSettings.brightnessLevel);
            OnBrightnessChanged = null;
            if((_brightnessSlider = GetComponentInChildren<Slider>()) is not null)
            {
                _brightnessSlider.value = PlayerPrefs.GetFloat(BrightnessKey);
                _brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
                ChangeBrightness(_brightnessSlider.value);
            }
        }


        private void ChangeBrightness(float brightnessValue)
        {
            OnBrightnessChanged?.Invoke(brightnessValue);
            PlayerPrefs.SetFloat(BrightnessKey, brightnessValue);
        }

        private void OnDestroy()
        {
            OnBrightnessChanged = null;
            PlayerPrefs.Save();
        }
    }
}
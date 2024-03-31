using System;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class BrightnessSettings : MonoBehaviour
    {
        private const string BrightnessKey = "Brightness";
        private Slider _brightnessSlider;
        public event Action<float> OnBrightnessChanged; 

        private void Awake()
        {
            if((_brightnessSlider = GetComponentInChildren<Slider>()) is not null)
            {
                _brightnessSlider.value = PlayerPrefs.GetFloat(BrightnessKey);
                _brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
                ChangeBrightness(_brightnessSlider.value);
            }
        }
    
        public void ChangeBrightness(float brightnessValue)
        {
            OnBrightnessChanged?.Invoke(brightnessValue);
            PlayerPrefs.SetFloat(BrightnessKey, brightnessValue);
        }

        private void OnDestroy()
        {
            PlayerPrefs.Save();
        }
    }
}
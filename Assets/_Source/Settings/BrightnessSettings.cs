using System;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class BrightnessSettings : MonoBehaviour
    {
        private const string BrightnessKey = "Brightness";
        private Slider _brightnessSlider;

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
            PlayerPrefs.SetFloat(BrightnessKey, brightnessValue);
        }

        private void OnDestroy()
        {
            PlayerPrefs.Save();
        }
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core
{
    public class SettingsMenuManager : MonoBehaviour
    {
        public TMP_Dropdown graphicsDropdown;
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;
        public AudioMixer mainAudioMixer;

        private void Start()
        {
            ChangeMasterVolume();
        }

        public void ChangeGraphicsQuality()
        {
            QualitySettings.SetQualityLevel(graphicsDropdown.value);
        }

        public void ChangeMasterVolume()
        {
            mainAudioMixer.SetFloat("MasterVolume", ConvertToVolume(masterVolumeSlider.value));
        }
        
        public void ChangeMusicVolume()
        {
            mainAudioMixer.SetFloat("MusicVolume", ConvertToVolume(musicVolumeSlider.value));
        }
        
        public void ChangeSfxVolume()
        {
            mainAudioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
        }

        private float ConvertToVolume(float sliderPosition)
        {
            float x = Mathf.Pow(sliderPosition, 0.2f);
            return Mathf.Lerp(-80, 0, x);
        }
    }
}

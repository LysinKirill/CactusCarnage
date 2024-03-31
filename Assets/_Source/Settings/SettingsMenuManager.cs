using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsMenuManager : MonoBehaviour
    {
        public TMP_Dropdown graphicsDropdown;
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;
        public AudioMixer mainAudioMixer;

        private const string QualityLevelKey = "QualitySettins";
        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SFXVolume";
        private void Start()
        {
            InitSliders();
            ChangeMasterVolume();
            ChangeMusicVolume();
            ChangeGraphicsQuality();
            ChangeSfxVolume();
            gameObject.SetActive(false);
        }

        private void InitSliders()
        {
            graphicsDropdown.value = PlayerPrefs.GetInt(QualityLevelKey);
            masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey);
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxVolumeKey);
        }

        public void ChangeGraphicsQuality()
        {
            QualitySettings.SetQualityLevel(graphicsDropdown.value);
            PlayerPrefs.SetInt(QualityLevelKey, graphicsDropdown.value);
        }

        public void ChangeMasterVolume()
        {
            var volume = ConvertToVolume(masterVolumeSlider.value);
            mainAudioMixer.SetFloat(MasterVolumeKey, volume);
            PlayerPrefs.SetFloat(MasterVolumeKey, masterVolumeSlider.value);
        }
        
        public void ChangeMusicVolume()
        {
            var volume = ConvertToVolume(musicVolumeSlider.value);
            mainAudioMixer.SetFloat(MusicVolumeKey, volume);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolumeSlider.value);
        }
        
        public void ChangeSfxVolume()
        {
            var volume = ConvertToVolume(sfxVolumeSlider.value);
            mainAudioMixer.SetFloat(SfxVolumeKey, sfxVolumeSlider.value);
            PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolumeSlider.value);
        }

        private float ConvertToVolume(float sliderPosition)
        {
            float x = Mathf.Pow(sliderPosition, 0.2f);
            return Mathf.Lerp(-80, 0, x);
        }
        
        private void OnDestroy()
        {
            PlayerPrefs.Save();
        }
    }
}
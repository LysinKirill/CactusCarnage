using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Core
{
    public class LevelController : MonoBehaviour
    {
        private PostProcessVolume _postProcessVolume;
        private AutoExposure _autoExposure;
        
        private const string BrightnessKey = "Brightness";
        private float _brightnessValue;

        private void Awake()
        {
            LoadBrightness();
            _postProcessVolume = GetComponentInChildren<PostProcessVolume>();

            var profile = _postProcessVolume.profile;
            if (profile.TryGetSettings(out _autoExposure))
                _autoExposure.keyValue.value = _brightnessValue;
        }

        private void LoadBrightness()
        {
            _brightnessValue = PlayerPrefs.GetFloat(BrightnessKey, 1);
        }
    }
}

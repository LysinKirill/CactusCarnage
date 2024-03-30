using Core.Player;
using Settings;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace Core.Controllers
{
    public class LevelController : MonoBehaviour
    {
        private PostProcessVolume _postProcessVolume;
        private AutoExposure _autoExposure;
        
        private const string BrightnessKey = "Brightness";
        private float _brightnessValue;

        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private BrightnessSettings brightnessSettings;

        private void Awake()
        {
            SetUpLevelVisuals();
            Subscribe();
        }

        private void Subscribe()
        {
            playerHealth.OnUpdateHealth += CheckLooseCondition;
            brightnessSettings.OnBrightnessChanged += UpdateBrightness;
        }

        private void CheckLooseCondition(int health)
        {
            if (health != 0)
                return;
            SceneManager.LoadScene(0);
        }
        private void SetUpLevelVisuals()
        {
            LoadBrightness();
            UpdateBrightness(_brightnessValue);
        }

        private void LoadBrightness()
        {
            _brightnessValue = PlayerPrefs.GetFloat(BrightnessKey, 1);
        }
        
        private void UpdateBrightness(float brightnessValue)
        {
            _postProcessVolume = GetComponentInChildren<PostProcessVolume>();
            var profile = _postProcessVolume.profile;
            if (profile.TryGetSettings(out _autoExposure))
                _autoExposure.keyValue.value = brightnessValue;
        }
    }
}

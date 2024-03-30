using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace Core
{
    public class LevelController : MonoBehaviour
    {
        private PostProcessVolume _postProcessVolume;
        private AutoExposure _autoExposure;
        
        private const string BrightnessKey = "Brightness";
        private float _brightnessValue;

        [SerializeField] private PlayerHealth playerHealth;

        private void Awake()
        {
            SetUpLevelVisuals();
            Subscribe();
        }

        private void Subscribe()
        {
            playerHealth.OnUpdateHealth += CheckLooseCondition;
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

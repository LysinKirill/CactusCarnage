using Core.Player;
using Settings;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Core.Controllers
{
    public class LevelController : MonoBehaviour
    {
        private PostProcessVolume _postProcessVolume;
        private AutoExposure _autoExposure;
        
        private const string BrightnessKey = "Brightness";
        private float _brightnessValue;
        private PlayerState _playerHealth;

        [SerializeField] private GameObject player;
        [SerializeField] private BrightnessSettings brightnessSettings;
        [SerializeField] private GameObject defeatPanel;
        [SerializeField] private GameObject canvas;

        private void Start()
        {
            defeatPanel.SetActive(false);
            UpdateBrightness(_brightnessValue);
        }
        private void Awake()
        {
            _playerHealth = player.GetComponent<PlayerState>();
            SetUpLevelVisuals();
            Subscribe();
        }

        private void Subscribe()
        {
            _playerHealth.OnUpdateHealth += CheckLooseCondition;
            brightnessSettings.OnBrightnessChanged += UpdateBrightness;
        }

        private void CheckLooseCondition(int health)
        {
            if (health > 0)
                return;
            foreach (Transform child in canvas.transform)
            {
                child.gameObject.SetActive(false);
            }
            defeatPanel.SetActive(true);
            player.SetActive(false);
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
            _brightnessValue = brightnessValue;
            _postProcessVolume = GetComponentInChildren<PostProcessVolume>();
            var profile = _postProcessVolume.profile;
            if (profile.TryGetSettings(out _autoExposure))
                _autoExposure.keyValue.value = brightnessValue;
        }
    }
}

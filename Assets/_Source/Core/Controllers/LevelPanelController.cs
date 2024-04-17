using Game;
using ScriptableObjects.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.Controllers
{
    public class LevelPanelController : MonoBehaviour
    {
        [SerializeField] private LevelPackAsset levelPack;
        [SerializeField] private GameObject levelSlotPrefab;

        [SerializeField] private Sprite lockedLevelIcon;
        [SerializeField] private Sprite unlockedLevelIcon;
        [SerializeField] private Sprite completedLevelIcon;
        
        private List<(string LevelName, GameObject Slot)> _levelSlots = new List<(string, GameObject)>();

        private void Awake()
        {
            for (int levelId = 0; levelId < levelPack.levels.Count; levelId++)
            {
                SceneField levelScene = levelPack.levels[levelId];
                var levelSlot = Instantiate(levelSlotPrefab, Vector3.zero, Quaternion.identity);
                levelSlot.transform.SetParent(gameObject.transform);
                levelSlot.transform.localScale = Vector3.one;
                _levelSlots.Add((levelScene.SceneName, levelSlot));

                var button = levelSlot.GetComponent<Button>();
                if (button is null)
                    continue;
                button.onClick.AddListener(() => Scenes.ChangeScene(levelScene));

                var buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = $"{levelId + 1}";
            }
        }

        private void Start()
        {
            Subscribe();
            InitializeAvailabilityIcons();
        }


        private GameObject GetSlotByLevelName(string levelName) =>
            _levelSlots.FirstOrDefault(x => x.LevelName == levelName).Slot;

        private void Subscribe()
        {
            var progressManager = PlayerProgressManager.Instance;
            progressManager.OnLevelStatusChanged += UpdateLevelAvailability;
        }

        private void UpdateLevelAvailability(string levelName, LevelStatus status)
        {
            var levelSlot = GetSlotByLevelName(levelName);
            if (levelSlot == null)
                return;

            Image levelStatusIcon = null;
            foreach(Transform child in levelSlot.transform)
                if (child.TryGetComponent(out levelStatusIcon))
                    break;
            if (levelStatusIcon == null)
                return;

            switch (status)
            {
                case LevelStatus.Locked:
                    levelStatusIcon.sprite = lockedLevelIcon;
                    LockLevel(levelSlot);
                    return;
                case LevelStatus.Unlocked:
                    levelStatusIcon.sprite = unlockedLevelIcon;
                    break;
                case LevelStatus.Completed:
                    levelStatusIcon.sprite = completedLevelIcon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
            UnlockLevel(levelSlot);
        }


        private void LockLevel(GameObject slot)
        {
            if (!slot.TryGetComponent(out Button button)
                || !slot.TryGetComponent(out Image image))
                return;

            var color = image.color;
            color.a = 150f / 255f;
            image.color = color;
            
            button.enabled = false;
        }
        
        private void UnlockLevel(GameObject slot)
        {
            if (!slot.TryGetComponent(out Button button)
                || !slot.TryGetComponent(out Image image))
                return;

            var color = image.color;
            color.a = 1f;
            image.color = color;
            
            button.enabled = true;
        }

        private void InitializeAvailabilityIcons()
        {
            var progressManager = PlayerProgressManager.Instance;

            foreach (var (levelName, _) in _levelSlots)
            {
                if (!progressManager.LevelExists(levelName))
                    continue;
                var levelStatus = progressManager.GetLevelStatus(levelName);
                UpdateLevelAvailability(levelName, levelStatus);
            }
        }
    }
}

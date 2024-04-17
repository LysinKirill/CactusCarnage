using ScriptableObjects.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers
{
    public class PlayerProgressManager : MonoBehaviour
    {
        [SerializeField] private LevelPackAsset levelPack;
        [SerializeField] private SceneAsset startLevel;

        private static PlayerProgressManager _instance;

        public string LastAvailableLevelName =>
            Levels.Last(x => x.Status != LevelStatus.Locked).LevelName;

        public List<(string LevelName, LevelStatus Status)> Levels { get; private set; } = new List<(string, LevelStatus)>();
        
        //public Dictionary<string, LevelStatus> LevelStatusMap { get; private set; }
        //    = new Dictionary<string, LevelStatus>();

        public event Action<string, LevelStatus> OnLevelStatusChanged;

        public static PlayerProgressManager Instance
        {
            get { return _instance; }
            set
            {
                if (_instance == null)
                    _instance = value;
            }
        }

        private PlayerProgressManager() { }

        private void Awake()
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
            LoadProgress();
        }

        private void Start()
        {
            UpdateUnlockedLevels();
        }

        public bool LevelExists(string levelName) => Levels.Any(x => x.LevelName == levelName);
        public LevelStatus GetLevelStatus(string levelName)
            => Levels.FirstOrDefault(x => x.LevelName == levelName).Status;
        
        private void LoadProgress()
        {
            foreach (var levelScene in levelPack.levels)
            {
                string levelName = levelScene.SceneName;
                if (!PlayerPrefs.HasKey(levelName))
                {
                    PlayerPrefs.SetInt(levelName, (int)LevelStatus.Locked);
                    PlayerPrefs.Save();
                }
                Levels.Add((levelName, (LevelStatus)PlayerPrefs.GetInt(levelName)));
            }
        }

        public void ChangeLevelStatus(string levelName, LevelStatus newLevelStatus)
        {
            PlayerPrefs.SetInt(levelName, (int)newLevelStatus);
            PlayerPrefs.Save();
            var levelItemId = Levels.FindIndex(x => x.LevelName == levelName);

            if (levelItemId == -1)
                Levels.Add((levelName, newLevelStatus));
            else
                Levels[levelItemId] = (levelName, newLevelStatus);

            UpdateUnlockedLevels();
            OnLevelStatusChanged?.Invoke(levelName, newLevelStatus);
        }

        public void ResetProgress()
        {
            levelPack.levels.ForEach(level =>
            {
                ChangeLevelStatus(level.SceneName, LevelStatus.Locked);
                PlayerPrefs.DeleteKey(level.SceneName);
            });
            PlayerPrefs.Save();
            UpdateUnlockedLevels();
        }

        private void UpdateUnlockedLevels()
        {
            int firstNotCompletedId = Levels.FindIndex(x => x.Status != LevelStatus.Completed);
            if (firstNotCompletedId == -1)
                return;

            var updatedLevelItem = (Levels[firstNotCompletedId].LevelName, LevelStatus.Unlocked);
            Levels[firstNotCompletedId] = updatedLevelItem;
            OnLevelStatusChanged?.Invoke(Levels[firstNotCompletedId].LevelName, LevelStatus.Unlocked);
        }
    }

    public enum LevelStatus
    {
        Locked = 0,
        Unlocked = 1,
        Completed = 2,
    }
}

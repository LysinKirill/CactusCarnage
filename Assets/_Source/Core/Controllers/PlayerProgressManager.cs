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
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }

        private void Start()
        {
            string levelName = startLevel.name;
            var levelItem = Levels.FirstOrDefault(x => x.Item1 == levelName);
            
            if (levelItem.LevelName == null
                || levelItem.Status == LevelStatus.Locked)
                ChangeLevelStatus(levelName, LevelStatus.Unlocked);
        }

        public bool LevelExists(string levelName) => Levels.Any(x => x.LevelName == levelName);
        public LevelStatus GetLevelStatus(string levelName)
            => Levels.FirstOrDefault(x => x.LevelName == levelName).Status;
        
        private void LoadProgress()
        {
            foreach (var levelScene in levelPack.levels)
            {
                string levelName = levelScene.name;
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
            var levelItem = Levels.FirstOrDefault(x => x.LevelName == levelName);

            if (levelItem.LevelName == null)
                Levels.Add((levelName, newLevelStatus));
            else
                levelItem.Status = newLevelStatus;

            OnLevelStatusChanged?.Invoke(levelName, newLevelStatus);
        }
    }

    public enum LevelStatus
    {
        Locked = 0,
        Unlocked = 1,
        Completed = 2,
    }
}

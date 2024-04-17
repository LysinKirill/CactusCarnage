using Core.Controllers;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Scenes : MonoBehaviour
    {
        [SerializeField] private SceneField mainMenuScene;
        

        public static void ChangeScene(SceneField scene)
        {
            SceneManager.LoadScene(scene.SceneName);
        }
        

        public void ReloadActiveScene()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        //public void LoadLevel(SceneAsset levelSceneAsset) => ChangeScene(levelSceneAsset);


        public void LoadLastAvailableLevel()
        {
            var progressManager = PlayerProgressManager.Instance;
            var lastAvailableLevelName = progressManager.LastAvailableLevelName;
            if (lastAvailableLevelName == null)
                return;
            SceneManager.LoadScene(lastAvailableLevelName);
        }

        public void LoadMainMenu() => ChangeScene(mainMenuScene);
    }
}


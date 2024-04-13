using Core.Controllers;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Scenes : MonoBehaviour
    {
        [SerializeField] private SceneAsset mainMenuScene;
        

        public static void ChangeScene(SceneAsset scene)
        {
            SceneManager.LoadScene(scene.name);
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


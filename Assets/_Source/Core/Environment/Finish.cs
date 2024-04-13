using Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Environment
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject player;
        [SerializeField] private LayerMask playerLayer;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & playerLayer) == 0)
                return;
            foreach (Transform child in canvas.transform)
            {
                child.gameObject.SetActive(false);
            }
            winPanel.SetActive(true);
            player.SetActive(false);

            var activeScene = SceneManager.GetActiveScene();
            PlayerProgressManager.Instance.ChangeLevelStatus(activeScene.name, LevelStatus.Completed);
        }
    }
}

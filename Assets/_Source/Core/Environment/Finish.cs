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
        private void OnTriggerEnter2D(Collider2D col)
        {
            foreach (var children in canvas.GetComponentsInChildren<Transform>())
            {
                children.gameObject.SetActive(false);
            }
            canvas.SetActive(true);
            winPanel.SetActive(true);
            foreach (var children in winPanel.GetComponentsInChildren<Transform>())
            {
                children.gameObject.SetActive(true);
            }
            player.SetActive(false);
        }
    }
}

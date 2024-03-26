using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemDescription : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        private void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            image.gameObject.SetActive(false);
            title.text = string.Empty;
            description.text = string.Empty;
        }

        public void SetDescription(
            Sprite sprite,
            string newName,
            string newDescription)
        {
            image.gameObject.SetActive(true);
            image.sprite = sprite;
            title.text = newName;
            description.text = newDescription;
        }
    }
}

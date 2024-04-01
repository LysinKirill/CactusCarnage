using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        public void AddButton(string buttonText, Action onClickAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponent<Button>().onClick.AddListener(() => onClickAction());
            button.GetComponentInChildren<TMPro.TMP_Text>().text = buttonText;
        }

        public void Toggle(bool isActive)
        {
            if (isActive)
                RemoveOldButtons();
            gameObject.SetActive(isActive);
        }

        public void RemoveOldButtons()
        {
            foreach(Transform childTransform in transform)
                Destroy(childTransform.gameObject);
        }
    }
}

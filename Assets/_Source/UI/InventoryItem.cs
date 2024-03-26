using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text quantity;
        [SerializeField] private Image frame;

        public event Action<InventoryItem> OnItemClicked;
        public event Action<InventoryItem> OnItemDropped;
        public event Action<InventoryItem> OnItemBeginDrag;
        public event Action<InventoryItem> OnItemEndDrag;
        public event Action<InventoryItem> OnRightMouseBtnClick;

        private bool _isEmpty = true;


        private void Awake()
        {
            Reset();
            Deselect();
        }

        public void Select()
        {
            frame.enabled = true;
        }

        public void OnBeginDrag()
        {
            if (_isEmpty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop()
        {
            OnItemDropped?.Invoke(this);
        }

        public void OnEndDrag()
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(BaseEventData data)
        {
            if(_isEmpty)
                return;
            PointerEventData pointerEventData = data as PointerEventData;
            if (pointerEventData?.button == PointerEventData.InputButton.Right)
                OnRightMouseBtnClick?.Invoke(this);
            else
                OnItemClicked?.Invoke(this);
        }

        private void Deselect()
        {
            frame.enabled = false;
        }

        private void Reset()
        {
            image.gameObject.SetActive(false);
            _isEmpty = true;
        }

        public void SetData(Sprite sprite, int itemQuantity = 1)
        {
            image.gameObject.SetActive(true);
            image.sprite = sprite;
            quantity.text = itemQuantity.ToString();
            _isEmpty = false;
        }
    }
}

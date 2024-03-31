using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventoryItem :
        MonoBehaviour,
        IPointerClickHandler,
        IBeginDragHandler,
        IEndDragHandler,
        IDropHandler,
        IDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text quantity;
        [SerializeField] private Image frame;

        public event Action<InventoryItem> OnItemClicked;
        public event Action<InventoryItem> OnItemDroppedOn;
        public event Action<InventoryItem> OnItemBeginDrag;
        public event Action<InventoryItem> OnItemEndDrag;
        public event Action<InventoryItem> OnRightMouseBtnClick;

        private bool _isEmpty = true;


        private void Awake()
        {
            ClearFollowers();
            Reset();
            Deselect();
        }

        private void ClearFollowers()
        {
            OnItemClicked = null;
            OnItemBeginDrag = null;
            OnItemEndDrag = null;
            OnItemDroppedOn = null;
            OnRightMouseBtnClick = null;
        }

        private void OnDestroy()
        {
            ClearFollowers();
        }

        public void Select()
        {
            frame.enabled = true;
        }
        

        public void Deselect()
        {
            if(frame.IsDestroyed())
                return;
            frame.enabled = false;
        }

        public void Reset()
        {
            if(image.IsDestroyed())
                return;
            image.gameObject.SetActive(false);
            _isEmpty = true;
        }

        
        public void SetData(Sprite sprite, int itemQuantity = 1)
        {
            if (image.IsDestroyed())
                return;
            image.gameObject.SetActive(true);
            image.sprite = sprite;
            quantity.text = itemQuantity.ToString();
            _isEmpty = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData?.button == PointerEventData.InputButton.Right)
                OnRightMouseBtnClick?.Invoke(this);
            else
                OnItemClicked?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isEmpty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Required by IBeginDragHandler
        }
    }
}

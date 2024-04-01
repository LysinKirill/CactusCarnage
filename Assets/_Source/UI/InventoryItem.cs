using System;
using TMPro;
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
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public TMP_Text Quantity { get; private set; }
        [field: SerializeField] public Image Frame { get; private set; }

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

        public void HideImage()
        {
            if (Image.IsDestroyed())
                return;
            Image.gameObject.SetActive(false);
        }

        public void ShowImage()
        {
            if (Image.IsDestroyed())
                return;
            Image.gameObject.SetActive(true);
        }

        private void ClearFollowers()
        {
            OnItemClicked = null;
            OnItemBeginDrag = null;
            OnItemEndDrag = null;
            OnItemDroppedOn = null;
            OnRightMouseBtnClick = null;
        }

        private void OnDestroy() => ClearFollowers();
        public void Select() => Frame.enabled = true;

        public void Deselect()
        {
            if(Frame.IsDestroyed())
                return;
            Frame.enabled = false;
        }

        public void Reset()
        {
            if(Image.IsDestroyed())
                return;
            Image.gameObject.SetActive(false);
            _isEmpty = true;
        }

        
        public void SetData(Sprite sprite, int itemQuantity = 1)
        {
            if (Image.IsDestroyed())
                return;
            Image.gameObject.SetActive(true);
            Image.sprite = sprite;
            Quantity.text = itemQuantity.ToString();
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

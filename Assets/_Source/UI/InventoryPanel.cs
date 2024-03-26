using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace UI
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField]
        private InventoryItem itemPrefab;
        [SerializeField]
        private RectTransform panel;
        [SerializeField]
        private ItemDescription itemDescription;
        [SerializeField]
        private ItemDragHandler dragHandler;

        private readonly List<InventoryItem> _menuItems = new List<InventoryItem>();

        public event Action<int> OnDescriptionRequested;
        public event Action<int> OnItemActionRequested;
        public event Action<int> OnStartDrag;
        public event Action<int, int> OnItemSwap;


        private int _currentlyDraggedId = -1;

        private void Start()
        {
            CloseInventory();
            itemDescription.ResetDescription();
            dragHandler.ExitFollower();
        }

        public void InitUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; ++i)
            {
                InventoryItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                newItem.transform.SetParent(panel);
                _menuItems.Add(newItem);
                SubscribeItem(newItem);
            }
        }

        public void UpdateItemView(int itemIndex, Sprite newSprite, int newQuantity)
        {
            if (itemIndex >= _menuItems.Count)
                return;
            _menuItems[itemIndex].SetData(newSprite, newQuantity);
        }

        public void UpdateItemView(int index, Sprite sprite, string itemName, string description)
        {
            DeselectAll();
            itemDescription.SetDescription(sprite, itemName, description);
            _menuItems[index].Select();
        }

        private void HandleShowItemActions(InventoryItem inventoryItem)
        {
            throw new NotImplementedException();
        }

        private void HandleEndDrag(InventoryItem inventoryItem) => dragHandler.ExitFollower();

        private void HandleSwap(InventoryItem inventoryItem)
        {
            int itemToSwapToIndex = _menuItems.IndexOf(inventoryItem);
            if (itemToSwapToIndex == -1)
                return;

            OnItemSwap?.Invoke(_currentlyDraggedId, itemToSwapToIndex);
            HandleItemSelection(inventoryItem);
        }

        private void HandleBeginDrag(InventoryItem inventoryItem)
        {
            _currentlyDraggedId = _menuItems.IndexOf(inventoryItem);
            if (_currentlyDraggedId == -1)
                return;
            
            HandleItemSelection(inventoryItem);
            OnStartDrag?.Invoke(_currentlyDraggedId);
        }

        private void HandleItemSelection(InventoryItem inventoryItem)
        {
            int selectedItemId = _menuItems.IndexOf(inventoryItem);
            if (selectedItemId == -1)
                return;
            
            OnDescriptionRequested?.Invoke(selectedItemId);
        }


        public void ShowInventory()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription();
            DeselectAll();
        }

        private void DeselectAll()
        {
            itemDescription.ResetDescription();
            _menuItems.ForEach(item => item.Deselect());
        }

        public void CloseInventory()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        private void ResetDraggedItem()
        {
            dragHandler.ExitFollower();
            _currentlyDraggedId = -1;
        }
        private void SubscribeItem(InventoryItem inventoryItem)
        {
            inventoryItem.OnItemClicked += HandleItemSelection;
            inventoryItem.OnItemBeginDrag += HandleBeginDrag;
            inventoryItem.OnItemDroppedOn += HandleSwap;
            inventoryItem.OnItemEndDrag += HandleEndDrag;
            inventoryItem.OnRightMouseBtnClick += HandleShowItemActions;
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            dragHandler.EnterFollower();
            dragHandler.SetData(sprite, quantity);
        }

        public void ResetAllItems()
        {
            foreach(var item in _menuItems)
            {
                item.Reset();
                item.Deselect();
            }
        }
    }
}

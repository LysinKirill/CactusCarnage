using Model;
using ScriptableObjects;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Core
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPanel inventoryPanel;
        [SerializeField] private InventoryAsset inventoryData;

        public List<InventoryItemModel> startingItems = new List<InventoryItemModel>();
        private void Start()
        {
            SetUpController();
            SetUpInventory();
        }

        private void SetUpInventory()
        {
            inventoryData.Init();
            inventoryData.OnInventoryUpdated += UpdateInventory;
            foreach (var item in startingItems)
            {
                if(item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventory(Dictionary<int, InventoryItemModel> updatedInventory)
        {
            inventoryPanel.ResetAllItems();
            foreach (var item in updatedInventory)
            {
                inventoryPanel.UpdateItemView(item.Key, item.Value.item.Sprite, item.Value.quantity);
            }
        }

        private void SetUpController()
        {
            inventoryPanel.InitUI(inventoryData.Size);
            inventoryPanel.OnDescriptionRequested += HandleDescriptionRequested;
            inventoryPanel.OnItemSwap+= HandleItemSwap;
            inventoryPanel.OnStartDrag += HandleDragging;
            inventoryPanel.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int index)
        {
            throw new System.NotImplementedException();
        }

        private void HandleDragging(int index)
        {
            InventoryItemModel item = inventoryData.GetItemAt(index);
            if(item.IsEmpty)
                return;

            inventoryPanel.CreateDraggedItem(item.item.Sprite, item.quantity);
        }

        private void HandleItemSwap(int firstItemIndex, int secondItemIndex)
        {
            inventoryData.SwapItems(firstItemIndex, secondItemIndex);
        }

        private void HandleDescriptionRequested(int index)
        {
            var inventoryItem = inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty)
                return;
            var itemAsset = inventoryItem.item;
            inventoryPanel.UpdateItemView(index, itemAsset.Sprite, itemAsset.name, itemAsset.Description);
        }
        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(!inventoryPanel.isActiveAndEnabled)
                {
                    inventoryPanel.ShowInventory();
                    foreach (var item in inventoryData.CurrentInventory)
                    {
                        inventoryPanel.UpdateItemView(
                            item.Key,
                            item.Value.item.Sprite,
                            item.Value.quantity
                        );
                    }
                }
                else
                {
                    inventoryPanel.CloseInventory();
                }
            }
        }
    }
}

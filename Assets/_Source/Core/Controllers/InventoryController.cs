using Model;
using ScriptableObjects.Items;
using System.Collections.Generic;
using System.Text;
using UI;
using UnityEngine;

namespace Core.Controllers
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPanel inventoryPanel;
        [SerializeField] private InventoryAsset inventoryData;
        [SerializeField] private AudioClip dropClip;
        [SerializeField] private AudioSource audioSource;

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
                if (item.IsEmpty)
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
            Subscribe();
        }

        private void Subscribe()
        {
            inventoryPanel.OnDescriptionRequested += HandleDescriptionRequested;
            inventoryPanel.OnItemSwap += HandleItemSwap;
            inventoryPanel.OnStartDrag += HandleDragging;
            inventoryPanel.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int index)
        {
            var inventoryItem = inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty)
                return;

            if (inventoryItem.item is IItemAction itemAction)
            {
                inventoryPanel.ShowItemAction(index);
                inventoryPanel.AddAction(itemAction.ActionName, () => PerformAction(index));
            }

            if (inventoryItem.item is IDestroyableItem)
                inventoryPanel.AddAction("Выбросить", () => DropItem(index, inventoryItem.quantity));
            
        }


        public void PerformAction(int index)
        {
            var inventoryItem = inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty)
                return;

            if (inventoryItem.item is IDestroyableItem)
                inventoryData.RemoveItem(index, 1);

            if (inventoryItem.item is IItemAction itemAction)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.ActionSfx);
                if (inventoryData.GetItemAt(index).IsEmpty)
                {
                    inventoryPanel.DeselectAll();
                }
            }


        }

        private void DropItem(int index, int quantity)
        {
            inventoryData.RemoveItem(index, quantity);
            inventoryPanel.DeselectAll();
            audioSource.PlayOneShot(dropClip);
        }

        private void HandleDragging(int index)
        {
            InventoryItemModel item = inventoryData.GetItemAt(index);
            if (item.IsEmpty)
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
            var description = PrepareDescription(inventoryItem);
            
            inventoryPanel.UpdateItemView(index, itemAsset.Sprite, itemAsset.Name, description);
        }

        private string PrepareDescription(InventoryItemModel inventoryItem)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(inventoryItem.item.Description);
            stringBuilder.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; ++i)
            {
                stringBuilder.Append(
                    $"{inventoryItem.itemState[i].itemParameter.ParameterName}:" +
                    $" {inventoryItem.itemState[i].value} /" +
                    $" {inventoryItem.item.DefaultParameterList[i].value}"
                );
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!inventoryPanel.isActiveAndEnabled)
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
            if(Input.GetKeyDown(KeyCode.Escape))
                inventoryPanel.CloseInventory();
        }
    }
}

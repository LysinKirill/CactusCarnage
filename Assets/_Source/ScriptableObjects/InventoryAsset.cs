using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "SO/newInventory")]
    public class InventoryAsset : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItemModel> inventoryItems;

        [field: SerializeField] public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItemModel>> OnInventoryUpdated;

        public void Init()
        {
            inventoryItems = new List<InventoryItemModel>();
            for (int i = 0; i < Size; ++i)
                inventoryItems.Add(InventoryItemModel.Empty);
        }

        public int AddItem(ItemAsset itemAsset, int quantity)
        {
            if (!itemAsset.IsStackable)
            {
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    while (quantity > 0 && !IsInventoryFull())
                    {
                        quantity -= AddItemToFirstEmptySlot(itemAsset, 1);
                    }

                    NotifyAboutUpdate();
                    return quantity;
                }

            }

            quantity = AddStackableItem(itemAsset, quantity);
            NotifyAboutUpdate();
            return quantity;
        }

        private int AddItemToFirstEmptySlot(ItemAsset itemAsset, int quantity)
        {
            InventoryItemModel newItem = new InventoryItemModel
            {
                quantity = quantity,
                item = itemAsset,
            };

            for (int i = 0; i < inventoryItems.Count; ++i)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }

            return 0;
        }

        private bool IsInventoryFull() => !inventoryItems.Any(item => item.IsEmpty);

        private int AddStackableItem(ItemAsset itemAsset, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; ++i)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].item.ID == itemAsset.ID)
                {
                    int availableSpace = itemAsset.StackSize - inventoryItems[i].quantity;

                    if (quantity > availableSpace)
                    {
                        inventoryItems[i] = inventoryItems[i] with { quantity = itemAsset.StackSize };
                        quantity -= availableSpace;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i] with { quantity = quantity + inventoryItems[i].quantity };
                        NotifyAboutUpdate();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, itemAsset.StackSize);
                quantity -= newQuantity;
                AddItemToFirstEmptySlot(itemAsset, newQuantity);
            }

            return quantity;
        }

        public void AddItem(InventoryItemModel item) => AddItem(item.item, item.quantity);

        public InventoryItemModel GetItemAt(int index) => inventoryItems[index];

        public Dictionary<int, InventoryItemModel> CurrentInventory
        {
            get
            {
                Dictionary<int, InventoryItemModel> dict = new Dictionary<int, InventoryItemModel>();
                for (int index = 0; index < inventoryItems.Count; index++)
                {
                    InventoryItemModel item = inventoryItems[index];
                    if (item.IsEmpty)
                        continue;
                    dict[index] = inventoryItems[index];
                }

                return dict;
            }
        }

        public void SwapItems(int firstItemIndex, int secondItemIndex)
        {
            if (firstItemIndex == -1 || secondItemIndex == -1)
                return;
            (inventoryItems[firstItemIndex], inventoryItems[secondItemIndex]) = (inventoryItems[secondItemIndex], inventoryItems[firstItemIndex]);
            NotifyAboutUpdate();
        }

        private void NotifyAboutUpdate() => OnInventoryUpdated?.Invoke(CurrentInventory);
    }
}

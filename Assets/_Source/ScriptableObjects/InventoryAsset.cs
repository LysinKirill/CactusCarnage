using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "SO/newInventory")]
    public class InventoryAsset : ScriptableObject
    {
        [SerializeField] private List<InventoryItemModel> inventoryItems;

        [field: SerializeField] public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItemModel>> OnInventoryUpdated; 
        public void Init()
        {
            inventoryItems = new List<InventoryItemModel>();
            for(int i = 0; i < Size; ++i)
                inventoryItems.Add(InventoryItemModel.Empty);
        }

        public void AddItem(ItemAsset itemAsset, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; ++i)
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = new InventoryItemModel
                    {
                        quantity = quantity,
                        item = itemAsset,
                    };
                    return;
                }
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
            if(firstItemIndex == - 1 || secondItemIndex == - 1)
                return;
            (inventoryItems[firstItemIndex], inventoryItems[secondItemIndex]) = (inventoryItems[secondItemIndex], inventoryItems[firstItemIndex]);
            NotifyAboutUpdate();
        }

        private void NotifyAboutUpdate() => OnInventoryUpdated?.Invoke(CurrentInventory);
    }
}

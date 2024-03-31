using ScriptableObjects.Items;
using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public record InventoryItemModel
    {
        public int quantity;
        public ItemAsset item;
        public List<ItemParameter> itemState;

        public bool IsEmpty
        {
            get { return item == null; }
        }

        public static InventoryItemModel Empty => new InventoryItemModel
        {
            quantity = 0,
            item = null,
            itemState = new List<ItemParameter>(),
        };
    }
}

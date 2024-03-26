using ScriptableObjects;
using System;

namespace Model
{
    [Serializable]
    public record InventoryItemModel
    {
        public int quantity;
        public ItemAsset item;

        public bool IsEmpty
        {
            get { return item == null; }
        }

        public static InventoryItemModel Empty => new InventoryItemModel
        {
            quantity = 0,
            item = null,
        };
    }
}

using ScriptableObjects;
using System;
using UnityEngine;

namespace Core.PickUpSystem
{
    public class PickUpSystem : MonoBehaviour
    {
        [SerializeField] private InventoryAsset inventoryData;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isActiveAndEnabled)
                return;
            PickableItem item = collision.GetComponent<PickableItem>();
            if (item == null)
                return;

            int remainder = inventoryData.AddItem(item.Item, item.Quantity);
            if (remainder == 0)
                item.DestroyItem();
            else
                item.Quantity = remainder;
        }
    }
}

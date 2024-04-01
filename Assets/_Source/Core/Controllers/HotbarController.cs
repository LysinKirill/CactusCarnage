using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Controllers
{
    public class HotbarController : MonoBehaviour
    {
        private List<GameObject> _slots = new List<GameObject>();

        private static readonly List<KeyCode> _keyCodes = new List<KeyCode>()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
        };
        
        // private void Awake()
        // {
        //     foreach (Transform child in transform)
        //     {
        //         child.
        //     }
        // }
        //
        //
        // public void PerformAction(int index)
        // {
        //     if (index >= _slots.Count || index < 0)
        //         return;
        //
        //     var slot = _slots[index];
        //     
        //     
        //     ////
        //     var inventoryItem = inventoryData.GetItemAt(index);
        //     if (inventoryItem.IsEmpty)
        //         return;
        //     
        //     IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        //     if (destroyableItem != null)
        //         inventoryData.RemoveItem(index, 1);
        //     
        //     IItemAction itemAction = inventoryItem.item as IItemAction;
        //     if (itemAction != null)
        //     {
        //         itemAction.PerformAction(gameObject, inventoryItem.itemState);
        //         audioSource.PlayOneShot(itemAction.ActionSfx);
        //         if (inventoryData.GetItemAt(index).IsEmpty)
        //         {
        //             inventoryPanel.DeselectAll();
        //         }
        //     }
        // }

    }
}

using ScriptableObjects.Items;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Core.Controllers
{
    public class HotbarController : MonoBehaviour
    {
        [SerializeField] private InventoryPanel inventoryPanel;
        [SerializeField] private InventoryAsset inventoryAsset;
        
        [SerializeField] private InventoryItem itemPrefab;
        [SerializeField] private int hotbarSize;
        [SerializeField] private RectTransform panel;
        [SerializeField] private GameObject player;
        
        private readonly List<InventoryItem> _items = new List<InventoryItem>();
        private readonly List<int> _trackInfo = new List<int>();
        
        
        
        private int _currentlyDraggedId = -1;

        private void Awake()
        {
            for (int i = 0; i < hotbarSize; ++i)
            {
                InventoryItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                Transform transform1;
                (transform1 = newItem.transform).SetParent(panel);
                transform1.localScale = Vector3.one;
                _items.Add(newItem);
                _trackInfo.Add(-1);
            }
        }

        private void Start()
        {
            inventoryPanel.OnStartDrag += UpdateCurrentlyDraggedId;
            foreach(var item in _items)
                SubscribeItem(item);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
                PerformAction(_trackInfo[0]);
            
            if(Input.GetKeyDown(KeyCode.Alpha2))
                PerformAction(_trackInfo[1]);
            
            if(Input.GetKeyDown(KeyCode.Alpha3))
                PerformAction(_trackInfo[2]);
        }
        
        
        private void UpdateCurrentlyDraggedId(int index)
        {
            _currentlyDraggedId = index;
        }



        private void SubscribeItem(InventoryItem inventoryItem)
        {
            inventoryItem.OnItemClicked += PerformAction;
            inventoryItem.OnItemDroppedOn += SetTrackedItem;
            inventoryAsset.OnInventoryUpdated += _ => UpdateHotbar();
        }

        private void PerformAction(InventoryItem item)
        {
            int index = _items.IndexOf(item);
            if (index == -1)
                return;
            
            PerformAction(_trackInfo[index]);
        }

        private void SetTrackedItem(InventoryItem item)
        {
            int index = _items.IndexOf(item);
            if (index == -1)
                return;

            _trackInfo[index] = _currentlyDraggedId;
            UpdateHotbar();
        }

        private void UpdateHotbar()
        {
            for (int i = 0; i < hotbarSize; ++i)
            {
                if(_trackInfo[i] == -1)
                    continue;

                var inventoryItem = inventoryAsset.GetItemAt(_trackInfo[i]);
                if (!inventoryItem.IsEmpty)
                {
                    _items[i].SetData(inventoryItem.item.Sprite, inventoryItem.quantity);
                    _items[i].ShowImage();
                }
                else
                    _items[i].HideImage();

            }
        }


        public void PerformAction(int index)
        {
            if (index == -1)
                return;

            var inventoryItem = inventoryAsset.GetItemAt(index);
            if (inventoryItem.IsEmpty)
                return;
            
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
                inventoryAsset.RemoveItem(index, 1);
            
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(player, inventoryItem.itemState);
                //audioSource.PlayOneShot(itemAction.ActionSfx);
                if (inventoryAsset.GetItemAt(index).IsEmpty)
                {
                    inventoryPanel.DeselectAll();
                }
            }
            UpdateHotbar();
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField]
        private InventoryItem itemPrefab;

        [SerializeField]
        private RectTransform panel;
        
        
        private List<InventoryItem> _menuItems = new List<InventoryItem>();


        public void InitUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; ++i)
            {
                InventoryItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                newItem.transform.SetParent(panel);
                _menuItems.Add(newItem);

                newItem.OnItemClicked += HandleItemSelection;
                newItem.OnItemBeginDrag += HandleBeginDrag;
                newItem.OnItemClicked += HandleSwap;
                newItem.OnItemClicked += HandleEndDrag;
                newItem.OnItemClicked += HandleShowItemActions;
            }
        }

        private void HandleShowItemActions(InventoryItem obj)
        {
            throw new NotImplementedException();
        }

        private void HandleEndDrag(InventoryItem obj)
        {
            throw new NotImplementedException();
        }

        private void HandleSwap(InventoryItem obj)
        {
            throw new NotImplementedException();
        }

        private void HandleBeginDrag(InventoryItem obj)
        {
            throw new NotImplementedException();
        }

        private void HandleItemSelection(InventoryItem obj)
        {
            Debug.Log(obj.name);
        }


        public void ShowInventory()
        {
            gameObject.SetActive(true);
        }

        public void CloseInventory()
        {
            gameObject.SetActive(false);
        }
    }
}

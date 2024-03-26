using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryPanel _inventoryPanel;

    [SerializeField]
    private int inventorySize;
    private void Start()
    {
        _inventoryPanel.InitUI(inventorySize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(!_inventoryPanel.isActiveAndEnabled)
                _inventoryPanel.ShowInventory();
            else
                _inventoryPanel.CloseInventory();
        }
    }
}

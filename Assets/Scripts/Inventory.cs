using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public event Action OnInventoryChanged;

    public void Add(InventoryItem item)
    {
        items.Add(item);
        OnInventoryChanged?.Invoke(); // Trigger the event when an item is added
    }

    public void DisableInventory()
    {
        // Disable the inventory system or relevant components here
        enabled = false; // For example, disable this script
    }
}

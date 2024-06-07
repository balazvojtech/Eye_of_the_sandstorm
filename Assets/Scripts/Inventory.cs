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
}

using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.Add(item); // Correctly call the Add method
                Destroy(gameObject); // Remove the item from the scene
            }
        }
    }
}

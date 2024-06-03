using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;  // Reference to the InventoryItem ScriptableObject

    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}

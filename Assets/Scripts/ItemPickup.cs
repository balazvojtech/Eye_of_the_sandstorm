using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            QuestManager questManager = FindObjectOfType<QuestManager>(); 

            if (inventory != null)
            {
                inventory.Add(item); // adding item to inventory
                if (questManager != null)
                {
                    questManager.CollectScript(); // reminding quest manager to show correct number of scripts collected
                }
                Destroy(gameObject); //destroy item upon pick-up
            }
        }
    }
}

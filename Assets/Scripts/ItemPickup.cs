using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;
    public AudioClip pickupSound; // pick-up sound

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
                if (pickupSound != null)
                {
                    GameObject audioObject = new GameObject("PickupSound");
                    AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                    audioSource.clip = pickupSound;
                    audioSource.Play();
                    Destroy(audioObject, pickupSound.length); //destroy audio object
                }
                Destroy(gameObject); //destroy item upon pick-up
            }
        }
    }
}

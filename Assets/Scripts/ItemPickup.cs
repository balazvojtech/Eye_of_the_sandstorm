using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;
    public AudioClip pickupSound; // Reference to the pickup sound

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            QuestManager questManager = FindObjectOfType<QuestManager>(); // Get reference to the QuestManager

            if (inventory != null)
            {
                inventory.Add(item); // Correctly call the Add method

                if (questManager != null)
                {
                    questManager.CollectScript(); // Notify the QuestManager
                }

                // Create an AudioSource to play the pickup sound
                if (pickupSound != null)
                {
                    GameObject audioObject = new GameObject("PickupSound");
                    AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                    audioSource.clip = pickupSound;
                    audioSource.Play();
                    
                    // Destroy the audio object after the sound has finished playing
                    Destroy(audioObject, pickupSound.length);
                }

                // Destroy the item immediately
                Destroy(gameObject);
            }
        }
    }
}

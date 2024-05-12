using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Prevent the player from walking through this object
            // For example, you can stop the player's movement or apply a force to push them back
            Debug.Log("Player collided with obstacle: " + gameObject.name);
        }
    }
}

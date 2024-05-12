using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include the namespace for TextMeshPro
using System.Collections;

public class Teleportation : MonoBehaviour
{
    // Reference to the black image for fading effect
    public Image fadeImage;

    // Reference to the UI TextMeshProUGUI element
    public TextMeshProUGUI teleportText; // Change the type to TextMeshProUGUI

    // Duration of the fade effect
    public float fadeDuration = 1f;

    // Delay before teleportation (in seconds)
    public float teleportDelay = 1f;

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger. Initiating teleportation...");

            // Start the fade effect and teleportation
            StartCoroutine(FadeAndTeleport(other.gameObject));
        }
    }

    private IEnumerator FadeAndTeleport(GameObject player)
    {
        Debug.Log("Fading to black...");

        // Fade to black
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        // Set text alpha to 0 at the beginning of the fade
        if (teleportText != null)
        {
            teleportText.alpha = 0f;
        }

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);

            // Increase the alpha of the text along with the black screen
            if (teleportText != null)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                teleportText.alpha = alpha;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;

        // Delay before teleportation
        yield return new WaitForSeconds(teleportDelay);

        Debug.Log("Player teleporting...");

        // Teleport the player
        if (player != null)
        {
            // Change the position of the player to the current position of the Teleportation GameObject
            player.transform.position = transform.position;
            Debug.Log("Player teleported to: " + transform.position);
        }
        else
        {
            Debug.LogWarning("Player is null!");
        }

        Debug.Log("Fading back to game...");

        // Fade back to game
        elapsedTime = 0f;
        startColor = targetColor;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);

            // Decrease the alpha of the text along with the black screen
            if (teleportText != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                teleportText.alpha = alpha;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;

        Debug.Log("Teleportation complete.");
    }
}

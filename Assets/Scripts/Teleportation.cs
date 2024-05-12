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

    // The location where the player will be teleported
    public Vector3 teleportLocation = new Vector3(-2.47f, 8.39f, -14.77f);

    private bool isTeleporting = false;

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player and if not currently teleporting
        if (other.CompareTag("Player") && !isTeleporting)
        {
            // Start the fade effect and teleportation
            StartCoroutine(FadeAndTeleport(other.gameObject));
        }
    }

    private IEnumerator FadeAndTeleport(GameObject player)
    {
        isTeleporting = true;

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

        // Teleport the player
        if (player != null)
        {
            // Change the position of the player to the specified teleportLocation
            player.transform.position = teleportLocation;
        }

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

        isTeleporting = false;
    }
}

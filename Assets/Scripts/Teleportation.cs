using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include the namespace for TextMeshPro
using System.Collections;

public class Teleportation : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI teleportText; 
    public float fadeDuration = 1f;
    public float teleportDelay = 1f;
    public Vector3 teleportLocation = new Vector3(-2.47f, 8.39f, -14.77f);

    private bool isTeleporting = false;

    private void OnTriggerExit(Collider other)
    {
        // check if the exiting collider is the player and if not currently teleporting if trying to leave city
        if (other.CompareTag("Player") && !isTeleporting)
        {
            // start the fade effect and teleportation
            StartCoroutine(FadeAndTeleport(other.gameObject));
        }
    }

    private IEnumerator FadeAndTeleport(GameObject player)
    {
        isTeleporting = true;

        // fade to black
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        // set text transparency to 0 at the start
        if (teleportText != null)
        {
            teleportText.alpha = 0f;
        }

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);

            // show the text 
            if (teleportText != null)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                teleportText.alpha = alpha;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;
        yield return new WaitForSeconds(teleportDelay);

        // teleport the player
        if (player != null)
        {
            // move player to set position
            player.transform.position = teleportLocation;
        }

        // fade back to game
        elapsedTime = 0f;
        startColor = targetColor;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);

            // fade the text out
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public Image blackImage;
    public TextMeshProUGUI[] introTexts;
    public AudioClip[] introAudioClips;
    public float fadeDuration = 1.0f;
    public float textFadeDuration = 0.5f;
    public float textDisplayDuration = 2.0f;

    private AudioManager2 audioManager2;

    void Start()
    {
        StartCoroutine(PlayIntroAndStartAudioManager());
    }

    IEnumerator PlayIntroAndStartAudioManager()
    {
        // Fade in black image
        blackImage.CrossFadeAlpha(1, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);

        // Display and fade in intro texts
        for (int i = 0; i < introTexts.Length; i++)
        {
            TextMeshProUGUI text = introTexts[i];
            AudioClip clip = introAudioClips.Length > i ? introAudioClips[i] : null; // Get the corresponding audio clip

            text.gameObject.SetActive(true);
            text.alpha = 0; // Set alpha to 0 initially
            StartCoroutine(FadeTextIn(text));

            // Play the audio clip
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            }

            yield return new WaitForSeconds(textFadeDuration + 0.1f); // Add a small delay for smooth transition

            // Wait for textDisplayDuration
            yield return new WaitForSeconds(textDisplayDuration);

            // Fade out the text
            StartCoroutine(FadeTextOut(text));
            yield return new WaitForSeconds(textFadeDuration);
            text.gameObject.SetActive(false);
        }

        // Fade out black image
        blackImage.CrossFadeAlpha(0, fadeDuration, false);

        // Start AudioManager2
        audioManager2 = FindObjectOfType<AudioManager2>(); // Find AudioManager2 in the scene
        if (audioManager2 != null)
        {
            audioManager2.PlayAudioSequence();
        }
    }

    IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        float elapsedTime = 0;
        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / textFadeDuration);
            text.alpha = t;
            yield return null;
        }
    }

    IEnumerator FadeTextOut(TextMeshProUGUI text)
    {
        float elapsedTime = 0;
        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(1 - (elapsedTime / textFadeDuration)); // Reverse Lerp
            text.alpha = t;
            yield return null;
        }
    }
}

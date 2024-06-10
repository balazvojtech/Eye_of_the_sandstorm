using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager3 : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; // Array to hold dialogue texts

    private AudioSource audioSource;
    public Text dialogueTextUI; // Reference to the UI text element
    public Image blackScreen; // Reference to the black screen UI element
    public float fadeDuration = 1.0f; // Duration of the fade

    private bool isPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        blackScreen.gameObject.SetActive(false); // Ensure the black screen is initially disabled
    }

    public void PlayAudioSequence()
    {
        StartCoroutine(PlayDelayedAudioClips());
    }

    private IEnumerator PlayDelayedAudioClips()
    {
        isPlaying = true;

        for (int i = 0; i < audioClips.Length; i++)
        {
            yield return new WaitForSeconds(clipDelays[i]); // Wait for the specified delay

            audioSource.clip = audioClips[i];
            audioSource.Play();
            dialogueTextUI.text = dialogueTexts[i]; // Update the UI text with the dialogue
            
            // Wait for the clip duration
            yield return new WaitForSeconds(audioClips[i].length);

            dialogueTextUI.text = ""; // Clear the UI text after the clip has finished playing
        }

        isPlaying = false;
        StartCoroutine(FadeToBlack()); // Start the fade to black
    }

    private IEnumerator FadeToBlack()
    {
        blackScreen.gameObject.SetActive(true); // Enable the black screen

        Color screenColor = blackScreen.color;
        screenColor.a = 0;
        blackScreen.color = screenColor;

        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            screenColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackScreen.color = screenColor;
            yield return null;
        }

        // Load the next scene after the fade is complete
        SceneManager.LoadScene("Glaciax");
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; // Array to hold dialogue texts

    private AudioSource audioSource;
    public Text dialogueTextUI; // Reference to the UI text element
    private bool isPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}

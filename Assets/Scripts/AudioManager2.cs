using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager2 : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; // Array to hold dialogue texts

    private AudioSource audioSource;
    public Text dialogueTextUI; // Reference to the UI text element
    private bool isPlaying = false;
    private QuestManager2 questManager2;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        questManager2 = FindObjectOfType<QuestManager2>(); // Find QuestManager2 in the scene
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
        questManager2.ShowContinueText(); // Show the "Press F to continue in the story" UI
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    // Example method to play the next dialogue, can be customized
    public void PlayNextDialogue()
    {
        // Add logic to play the next part of the story
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager4 : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; 

    public Text dialogueTextUI; 

    private AudioSource audioSource;
    private bool isPlaying = false;

    private CharacterMovement playerMovement; // Reference to PlayerMovement script
    private GameManager gameManager; // Reference to GameManager script

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = FindObjectOfType<CharacterMovement>(); // Find the PlayerMovement script in the scene
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager script in the scene
    }

    public void PlayAudioSequence()
    {
        StartCoroutine(PlayAudioSequenceCoroutine());
    }

    private IEnumerator PlayAudioSequenceCoroutine()
    {
        isPlaying = true;

        for (int i = 0; i < audioClips.Length; i++)
        {
            yield return new WaitForSeconds(clipDelays[i]); // Wait for the specified delay

            audioSource.clip = audioClips[i];
            audioSource.Play();
            dialogueTextUI.text = dialogueTexts[i]; // Update the UI text with the dialogue
            dialogueTextUI.gameObject.SetActive(true); // Show the dialogue text UI

            // Wait for the clip duration
            yield return new WaitForSeconds(audioClips[i].length);

            dialogueTextUI.gameObject.SetActive(false); // Hide the dialogue text UI after the clip has finished playing
        }

        isPlaying = false;

        // Disable player movement after audio finishes
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        else
        {
            Debug.LogError("PlayerMovement script not found in the scene.");
        }

        // Adjust the transparency of the display text after audio finishes
        if (gameManager != null)
        {
            gameManager.AdjustTextTransparency(1f); // Set transparency to 100%
        }
        else
        {
            Debug.LogError("GameManager script not found in the scene.");
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}

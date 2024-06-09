using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float[] delaysBetweenClips; // Array to store the delays between each audio clip in seconds

    private int currentClipIndex = 0;

    private void Start()
    {
        // Check if there are any audio clips
        if (audioClips.Length > 0 && audioClips.Length == delaysBetweenClips.Length)
        {
            // Start playing the audio clips with specified delays
            StartCoroutine(PlayAudioClipsWithDelays());
        }
        else
        {
            Debug.LogWarning("Number of audio clips and delays must be the same.");
        }
    }

    private IEnumerator PlayAudioClipsWithDelays()
    {
        // Loop through each audio clip
        for (int i = 0; i < audioClips.Length; i++)
        {
            // Play the current audio clip
            PlayCurrentClip();

            // Wait for the specified delay
            yield return new WaitForSeconds(delaysBetweenClips[i]);
        }
    }

    private void PlayCurrentClip()
    {
        // Set the next clip to play
        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();

        // Increment the clip index for the next clip
        currentClipIndex++;

        // Check if all clips have been played
        if (currentClipIndex >= audioClips.Length)
        {
            // Loop back to the beginning if all clips have been played
            currentClipIndex = 0;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; // list of dialogue texts

    private AudioSource audioSource;
    public Text dialogueTextUI; 
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
            yield return new WaitForSeconds(clipDelays[i]); // wait for the specified delay
            audioSource.clip = audioClips[i];
            audioSource.Play();
            dialogueTextUI.text = dialogueTexts[i]; // updating text with the one being spoken
            yield return new WaitForSeconds(audioClips[i].length);
            dialogueTextUI.text = ""; // clear text after audio ends
        }

        isPlaying = false;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager2 : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; // list of dialogue texts

    private AudioSource audioSource;
    public Text dialogueTextUI; // UI text element
    private bool isPlaying = false;
    private QuestManager2 questManager2;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        questManager2 = FindObjectOfType<QuestManager2>(); 
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
            dialogueTextUI.text = dialogueTexts[i]; // updating text 
            
            // waiting until clip finishes
            yield return new WaitForSeconds(audioClips[i].length);

            dialogueTextUI.text = ""; // clear text after audio ends
        }

        isPlaying = false;
        questManager2.ShowContinueText(); // Show the "Press F to continue in the story" UI
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}
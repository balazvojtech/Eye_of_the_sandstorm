using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager3 : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] clipDelays;
    public string[] dialogueTexts; // list of dialogue texts

    private AudioSource audioSource;
    public Text dialogueTextUI; 
    public Image blackScreen; 
    public float fadeDuration = 1.0f; 

    private bool isPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        blackScreen.gameObject.SetActive(false); // black screen is disabled at the start
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
            yield return new WaitForSeconds(clipDelays[i]); 

            audioSource.clip = audioClips[i];
            audioSource.Play();
            dialogueTextUI.text = dialogueTexts[i]; // updating UI with the text
            yield return new WaitForSeconds(audioClips[i].length);
            dialogueTextUI.text = ""; 
        }

        isPlaying = false;
        StartCoroutine(FadeToBlack()); 
    }

    private IEnumerator FadeToBlack()
    {
        blackScreen.gameObject.SetActive(true); // show black screen

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

        // load next scene - glaciax
        SceneManager.LoadScene("Glaciax");
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; 

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; 
    public float fadeDuration = 1.0f; 
    private int totalScripts = 7;
    private int collectedScripts = 0;
    public GameObject backgroundMusicObject; 
    public AudioClip newBackgroundMusicClip; // new background music 
    public AudioManager audioManager; 
    public Image fadeImage; // black image for fade effect

    private void Start()
    {
        UpdateQuestText();
        fadeImage.color = new Color(0, 0, 0, 0); // make the  image black and transparent
    }

    public void CollectScript()
    {
        collectedScripts++;
        UpdateQuestText();
        
        if (collectedScripts >= totalScripts)
        {
            StartCoroutine(EndGameSequence());
        }
    }

    private void UpdateQuestText()
    {
        questText.text = $"Collect hidden scripts ({collectedScripts}/{totalScripts})"; // showing how many scripts player has collected
    }

    private IEnumerator EndGameSequence()
    {
        yield return new WaitForSeconds(10.0f); // wait for 10 seconds before starting the fade - so that the dialogue of picked script can end

        // fade out the quest text
        StartCoroutine(FadeOutQuestText());

        // trigger dropping of machines
        MachineDropper[] machineDroppers = FindObjectsOfType<MachineDropper>();
        foreach (MachineDropper dropper in machineDroppers)
        {
            dropper.DropMachine();
        }

        yield return new WaitForSeconds(7.0f);

        // change background music clip
        if (backgroundMusicObject != null && newBackgroundMusicClip != null)
        {
            BackgroundMusic backgroundMusic = backgroundMusicObject.GetComponent<BackgroundMusic>();
            if (backgroundMusic != null)
            {
                backgroundMusic.ChangeBackgroundMusic(newBackgroundMusicClip);
            }
        }

        // play audio sequence
        if (audioManager != null)
        {
            audioManager.PlayAudioSequence();
        }

        // wait for the audio sequence to complete
        yield return new WaitWhile(() => audioManager.IsPlaying());

        // fade to black and load the next scene in the background
        StartCoroutine(FadeToBlackAndLoadScene());
    }

    private IEnumerator FadeOutQuestText()
    {
        float timer = 0f;
        Color startColor = questText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;
            questText.color = Color.Lerp(startColor, endColor, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        questText.color = endColor;
    }

    private IEnumerator FadeToBlackAndLoadScene()
    {
        float timer = 0f;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, endColor, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = endColor;
        SceneManager.LoadScene("Spaceship");
    }
}

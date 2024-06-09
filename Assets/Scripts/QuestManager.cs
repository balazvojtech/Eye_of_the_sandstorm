using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // Add this line for scene management

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; // Reference to the UI Text element
    public float fadeDuration = 1.0f; // Duration of the fade effect
    private int totalScripts = 7;
    private int collectedScripts = 0;
    public GameObject backgroundMusicObject; // Reference to the GameObject containing BackgroundMusic script
    public AudioClip newBackgroundMusicClip; // The new background music clip to be played
    public AudioManager audioManager; // Reference to the AudioManager script
    public Image fadeImage; // UI Image used for fading to black

    private void Start()
    {
        UpdateQuestText();
        fadeImage.color = new Color(0, 0, 0, 0); // Ensure the fade image is initially transparent
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
        questText.text = $"Collect hidden scripts ({collectedScripts}/{totalScripts})";
    }

    private IEnumerator EndGameSequence()
    {
        yield return new WaitForSeconds(10.0f); // Wait for 10 seconds before starting the fade

        // Fade out the quest text
        StartCoroutine(FadeOutQuestText());

        // Trigger dropping of machines
        MachineDropper[] machineDroppers = FindObjectsOfType<MachineDropper>();
        foreach (MachineDropper dropper in machineDroppers)
        {
            dropper.DropMachine();
        }

        yield return new WaitForSeconds(7.0f);

        // Change background music clip
        if (backgroundMusicObject != null && newBackgroundMusicClip != null)
        {
            BackgroundMusic backgroundMusic = backgroundMusicObject.GetComponent<BackgroundMusic>();
            if (backgroundMusic != null)
            {
                backgroundMusic.ChangeBackgroundMusic(newBackgroundMusicClip);
            }
        }

        // Play audio sequence
        if (audioManager != null)
        {
            audioManager.PlayAudioSequence();
        }

        // Wait for the audio sequence to complete
        yield return new WaitWhile(() => audioManager.IsPlaying());

        // Fade to black and load the next scene
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

        // Ensure the text is completely faded out
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

        // Ensure the screen is completely black
        fadeImage.color = endColor;

        // Load the "Spaceship" scene
        SceneManager.LoadScene("Spaceship");
    }
}

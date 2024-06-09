using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; // Reference to the UI Text element
    public float fadeDuration = 1.0f; // Duration of the fade effect
    private int totalScripts = 7;
    private int collectedScripts = 0;
    public AudioManager audioManager; // Reference to the AudioManager script

    private void Start()
    {
        UpdateQuestText();
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

        // Play audio sequence
        audioManager.PlayAudioSequence();
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
}

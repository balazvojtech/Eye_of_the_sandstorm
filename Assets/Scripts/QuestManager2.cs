using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager2 : MonoBehaviour
{
    public GameObject continueTextUI; // Reference to the "Press F to continue in the story" UI element
    public AudioManager2 audioManager2;
    public AudioManager3 audioManager3; // Reference to AudioManager3

    private bool canContinue = false;

    void Start()
    {
        continueTextUI.SetActive(false); // Ensure the UI element is initially disabled
        audioManager2 = FindObjectOfType<AudioManager2>(); // Find AudioManager2 in the scene
        audioManager3 = FindObjectOfType<AudioManager3>(); // Find AudioManager3 in the scene
    }

    void Update()
    {
        if (canContinue && Input.GetKeyDown(KeyCode.F))
        {
            ContinueStory();
        }
    }

    public void ShowContinueText()
    {
        continueTextUI.SetActive(true); // Enable the UI element
        canContinue = true;
    }

    private void ContinueStory()
    {
        continueTextUI.SetActive(false); // Disable the UI element
        canContinue = false;

        // Start AudioManager3 for the next part of the story
        if (audioManager3 != null)
        {
            audioManager3.PlayAudioSequence();
        }
    }
}

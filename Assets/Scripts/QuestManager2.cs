using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager2 : MonoBehaviour
{
    public GameObject continueTextUI; // Reference to the "Press F to continue in the story" UI element
    public AudioManager2 audioManager2;
    public AudioManager3 audioManager3; 

    private bool canContinue = false;

    void Start()
    {
        continueTextUI.SetActive(false); // at the start must be disabled
        audioManager2 = FindObjectOfType<AudioManager2>(); 
        audioManager3 = FindObjectOfType<AudioManager3>(); 
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
        continueTextUI.SetActive(true); // show UI
        canContinue = true;
    }

    private void ContinueStory()
    {
        continueTextUI.SetActive(false); // disable UI
        canContinue = false;

        // start AudioManager3 to continue in the story
        if (audioManager3 != null)
        {
            audioManager3.PlayAudioSequence();
        }
    }
}

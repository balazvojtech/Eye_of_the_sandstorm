using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject fadePanel;
    public GameObject menuCanvas; // Reference to your menu canvas
    public AudioSource menuMusic; // Reference to the MenuMusic audio source
    public float fadeSpeed = 1f;
    public float blackScreenDelay = 2f; // Delay in seconds before fading out from black

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void PlayButton()
    {
        StartCoroutine(FadeOutAndLoadScene("Game"));
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Player has quit the game.");
    }

    IEnumerator FadeIn()
    {
        fadePanel.SetActive(true);
        menuCanvas.SetActive(true); // Activate the menu canvas
        Image image = fadePanel.GetComponent<Image>();
        image.canvasRenderer.SetAlpha(1f);
        image.CrossFadeAlpha(0f, fadeSpeed, false);

        // Fade in the MenuMusic audio
        StartCoroutine(FadeAudio(menuMusic, 0f, 1f, fadeSpeed));

        yield return new WaitForSeconds(fadeSpeed);
        fadePanel.SetActive(false);
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        fadePanel.SetActive(true);
        Image image = fadePanel.GetComponent<Image>();
        image.canvasRenderer.SetAlpha(0f);
        image.CrossFadeAlpha(1f, fadeSpeed, false);

        // Fade out the MenuMusic audio
        StartCoroutine(FadeAudio(menuMusic, menuMusic.volume, 0f, fadeSpeed));

        yield return new WaitForSeconds(fadeSpeed);

        // Hold the black screen for the specified delay
        yield return new WaitForSeconds(blackScreenDelay);

        // Start loading the game scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the new scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Unload the current scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Fade in from black
        image.CrossFadeAlpha(0f, fadeSpeed, false);
        yield return new WaitForSeconds(fadeSpeed);

        // Deactivate the fadePanel
        fadePanel.SetActive(false);

        // Deactivate the menu canvas
        menuCanvas.SetActive(false);
    }

    IEnumerator FadeAudio(AudioSource audioSource, float startVolume, float endVolume, float fadeDuration)
    {
        float currentTime = 0f;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, currentTime / fadeDuration);
            yield return null;
        }
        audioSource.volume = endVolume;
    }
}

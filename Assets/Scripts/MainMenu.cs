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
        yield return new WaitForSeconds(blackScreenDelay);

        // start loading the game scene in the background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // wait until it fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // unload main menu - scene switch
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        image.CrossFadeAlpha(0f, fadeSpeed, false);
        yield return new WaitForSeconds(fadeSpeed);

        fadePanel.SetActive(false);

        menuCanvas.SetActive(false); //deactivate menu
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

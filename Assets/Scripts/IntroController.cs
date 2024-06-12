using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public Image blackImage;
    public TextMeshProUGUI[] introTexts;
    public AudioClip[] introAudioClips;
    public float fadeDuration = 1.0f;
    public float textFadeDuration = 0.5f;
    public float textDisplayDuration = 2.0f;

    public AudioManager4 audioManager4; // Reference to AudioManager4

    void Start()
    {
        StartCoroutine(StartIntro());
    }

    IEnumerator StartIntro()
    {
        // Disable input
        Input.ResetInputAxes();

        // fade in to black image
        blackImage.CrossFadeAlpha(1, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);

        // show intro texts
        for (int i = 0; i < introTexts.Length; i++)
        {
            TextMeshProUGUI text = introTexts[i];
            AudioClip clip = introAudioClips.Length > i ? introAudioClips[i] : null; // play audio clips in order

            text.gameObject.SetActive(true);
            text.alpha = 0; // set alpha to 0 at the start
            StartCoroutine(FadeTextIn(text));

            // play audio 
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            }

            yield return new WaitForSeconds(textFadeDuration + 0.1f); // small delay so that text doesnt show immediately

            // wait for textDisplayDuration
            yield return new WaitForSeconds(textDisplayDuration);

            // fade out the text
            StartCoroutine(FadeTextOut(text));
            yield return new WaitForSeconds(textFadeDuration);
            text.gameObject.SetActive(false);
        }

        // fade out black image
        blackImage.CrossFadeAlpha(0, fadeDuration, false);

        // enable input - movement + camera
        yield return new WaitForSeconds(fadeDuration);
        Input.ResetInputAxes();

        if (audioManager4 != null)
        {
            yield return new WaitForSeconds(fadeDuration); // wait for the black screen to fade out
            audioManager4.PlayAudioSequence(); // start playing audio from AudioManager4 after the fade-out of black screen
        }
    }

    IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        float elapsedTime = 0;
        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / textFadeDuration);
            text.alpha = t;
            yield return null;
        }
    }

    IEnumerator FadeTextOut(TextMeshProUGUI text)
    {
        float elapsedTime = 0;
        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(1 - (elapsedTime / textFadeDuration)); // Reverse Lerp
            text.alpha = t;
            yield return null;
        }
    }
}

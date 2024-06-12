using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScriptCollector : MonoBehaviour
{
    public Text itemNameText;
    public Text descriptionText;
    public float displayDuration = 3f;
    public float fadeDuration = 0.5f; 

    private Color transparentColor = new Color(1f, 1f, 1f, 0f); // transparent color
    private Color opaqueColor = new Color(1f, 1f, 1f, 1f); // seenable color

    private void Start()
    {
        // set the initial transparency of the text elements to 0
        itemNameText.color = transparentColor;
        descriptionText.color = transparentColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Script"))
        {
            // get the ScriptableObjectHolder component from the collected script object
            ScriptableObjectHolder holder = other.GetComponent<ScriptableObjectHolder>();

            if (holder != null && holder.inventoryItem != null)
            {
                // Get the item name and description from the ScriptableObjectHolder
                string itemName = holder.inventoryItem.itemName;
                string description = holder.inventoryItem.description;

                // update the UI text  with the script information
                itemNameText.text = itemName;
                descriptionText.text = description;

                // activate the UI text 
                itemNameText.gameObject.SetActive(true);
                descriptionText.gameObject.SetActive(true);

                // Start to fade in the text elements
                StartCoroutine(FadeTextElements(true));

                // Start to fade out the text elements after a certain duration
                StartCoroutine(FadeTextElementsAfterDelay(false, displayDuration));
            }
        }
    }

    IEnumerator FadeTextElements(bool fadeIn)
    {
        float timer = 0f;
        Color startColor = fadeIn ? transparentColor : opaqueColor;
        Color endColor = fadeIn ? opaqueColor : transparentColor;

        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;
            itemNameText.color = Color.Lerp(startColor, endColor, progress);
            descriptionText.color = Color.Lerp(startColor, endColor, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        itemNameText.color = endColor;
        descriptionText.color = endColor;
    }

    IEnumerator FadeTextElementsAfterDelay(bool fadeIn, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeTextElements(fadeIn));
    }
}

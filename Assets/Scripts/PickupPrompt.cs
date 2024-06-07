using UnityEngine;
using UnityEngine.UI;

public class PickupPrompt : MonoBehaviour
{
    public Camera cameraReference; // Reference to the specific camera
    public float interactionDistance = 2.0f; // Distance to interact with objects
    public Text pickUpText; // Reference to the UI Text element
    public float textTransparency = 0.8f; // Transparency of the text when enabled

    void Start()
    {
        // Set the transparency of the pickUpText at the start
        SetTextTransparency(0f);
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraReference.transform.position, cameraReference.transform.forward, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Block"))
            {
                // Display the pickup prompt if looking at a block
                ShowPickupPrompt();
            }
            else
            {
                // Hide the pickup prompt if not looking at a block
                HidePickupPrompt();
            }
        }
        else
        {
            // Hide the pickup prompt if not looking at anything
            HidePickupPrompt();
        }
    }

    void ShowPickupPrompt()
    {
        // Enable the UI Text element
        pickUpText.gameObject.SetActive(true);
        // Set the transparency of the UI Text
        SetTextTransparency(textTransparency);
    }

    void HidePickupPrompt()
    {
        // Disable the UI Text element
        pickUpText.gameObject.SetActive(false);
        // Set the transparency of the UI Text back to 0
        SetTextTransparency(0f);
    }

    void SetTextTransparency(float alpha)
    {
        Color textColor = pickUpText.color;
        textColor.a = alpha;
        pickUpText.color = textColor;
    }
}

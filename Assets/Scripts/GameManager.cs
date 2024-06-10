using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text displayText;

    // Method to adjust the transparency of the display text
    public void AdjustTextTransparency(float alphaValue)
    {
        Color textColor = displayText.color;
        textColor.a = alphaValue;
        displayText.color = textColor;
    }
}

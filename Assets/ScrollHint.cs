using UnityEngine;
using UnityEngine.UI;  // Required for Image
using TMPro;

public class ScrollHint : MonoBehaviour
{
    [TextArea]
    [Tooltip("The hint message to display when the player is near this scroll")]
    public string hintMessage = "Press E to pick up an item";

    [Tooltip("Reference to the TextMeshProUGUI element on your Canvas")]
    public TextMeshProUGUI hintText;

    [Tooltip("Optional: Image to display behind the text (e.g., scroll graphic)")]
    public Image scrollBackground;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintText != null)
            {
                hintText.text = hintMessage;
                hintText.gameObject.SetActive(true);
            }

            if (scrollBackground != null)
            {
                scrollBackground.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintText != null)
            {
                hintText.text = "";
                hintText.gameObject.SetActive(false);
            }

            if (scrollBackground != null)
            {
                scrollBackground.gameObject.SetActive(false);
            }
        }
    }
}

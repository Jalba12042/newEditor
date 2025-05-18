using UnityEngine;
using TMPro;  // Make sure you have TMP imported

public class ScrollHint : MonoBehaviour
{
    [TextArea]
    [Tooltip("The hint message to display when the player is near this scroll")]
    public string hintMessage = "Press E to pick up an item";

    [Tooltip("Reference to the TextMeshProUGUI element on your Canvas")]
    public TextMeshProUGUI hintText;

    // Called when a collider enters the trigger area
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))  // Make sure the player has the "Player" tag
        {
            if(hintText != null)
            {
                hintText.text = hintMessage;
                hintText.gameObject.SetActive(true);  // Show the text element
            }
        }
    }

    // Called when a collider exits the trigger area
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(hintText != null)
            {
                hintText.text = "";
                hintText.gameObject.SetActive(false);  // Hide the text element
            }
        }
    }
}



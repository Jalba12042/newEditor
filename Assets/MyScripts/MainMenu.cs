using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when the Play button is clicked
    public void PlayGame()
    {
        // Load the next scene in the build settings (index +1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Called when the Settings button is clicked
    public void OpenSettings()
    {
        // You can add settings UI logic here later
        Debug.Log("Settings menu not implemented yet.");
    }

    // Called when the Quit button is clicked
    public void QuitGame()
    {
        // This will quit the application
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

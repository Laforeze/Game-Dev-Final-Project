using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Controls()
    {
        Cursor.visible = true;
        Debug.Log("Controls Selected");
        SceneManager.LoadScene("Controls");
    }

    public void StartGame()
    {
        Cursor.visible = true;
        Debug.Log("Start Game Clicked");
        SceneManager.LoadScene("Cyber Outlaw");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void RestartGame()
    {
        Debug.Log("Begin again...");
        SceneManager.LoadScene("Cyber Outlaw");
    }

    public void MainMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("StartScreen");
    }
}
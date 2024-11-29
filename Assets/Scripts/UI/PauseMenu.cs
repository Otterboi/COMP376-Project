using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;  // Reference to the PauseMenu GameObject
    private bool isPaused = false;  // Check if the game is paused


    // Disable PauseMeu if in MainMenu
    void Start()
    {
        Time.timeScale = 1f; // Set game speed to normal (so it's not paused on lad)
        // Check if the current scene is the MainMenu
        if (SceneManager.GetActiveScene().name == "Test Main Menu")
        {
            // Disable the PauseMenu
            pauseMenuUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle between pausing and resuming the game with Esc
            if (isPaused)
            {
                Resume();
                Cursor.visible = false;
            }
            else
            {
                Pause();
                Cursor.visible = true;
            }
        }
    }

    // Method to resume the game
    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuUI.SetActive(false); // Hide PauseMenu
        Time.timeScale = 1f; // Set game speed to normal
        isPaused = false;  
    }

    // Method to pause the game
    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show PauseMenu
        Time.timeScale = 0f; // Set game speed to freeze 
        isPaused = true;
    }
}

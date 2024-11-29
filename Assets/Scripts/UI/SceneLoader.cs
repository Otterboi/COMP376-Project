using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    // Main Menu Screen
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


    // Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        AudioManager.Instance.PlayMusic();
        SceneManager.LoadScene("Level 1");
    }


}
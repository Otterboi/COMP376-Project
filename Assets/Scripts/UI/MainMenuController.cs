using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
   
    public GameObject panel; // Reference to the panel GameObject
    public GameObject howToPlayPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI pressStart; // Reference to the TextMeshPro Text
   
    public float flashTime; // Time before flashing TextMeshPro Text                   
    private Coroutine flashCoroutine; // Reference to the coroutine
    private FadeInOut fade;
    private bool isOpen;
   

    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<FadeInOut>();
        flashCoroutine = StartCoroutine(FlashText()); // Start the coroutine to flash the text
        isOpen = false;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            // Check if any key is pressed
            if (Input.anyKeyDown)
            {
                StopCoroutine(flashCoroutine); // Stop the flashing coroutine
                pressStart.gameObject.SetActive(false); // Hide "Press a Button" text                                        
                OpenPanel(); // Opens Menu
                isOpen = true;
            }
        }
        
    }

    // Method to open the panel
    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    // Coroutine to flash the text
    IEnumerator FlashText()
    {
        while (true)
        {
            // Toggle the text's visibility
            pressStart.gameObject.SetActive(!pressStart.gameObject.activeSelf);
            yield return new WaitForSeconds(flashTime);
        }
    }

    // Method to load scenes
    public void Level0()
    {
        Cursor.visible = false;
        Debug.Log("Tutorial method called");
        fade.FadeOut();
        SceneManager.LoadScene("Level 0");
    }

    // Main Menu Screen
    public void MainMenu()
    {
        Debug.Log("MAinMenu method called");
        SceneManager.LoadScene("Test Main Menu");
    }

    public void HowToPlay()
    {
        panel.SetActive(false);
        title.gameObject.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void Return()
    {
        title.gameObject.SetActive(false);
        panel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }

    // Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button playButton;
    Button quitButton;
    Button backButton;

    GameObject mainMenuCanvas;

    LevelMenu levelMenu;

    [SerializeField] Texture2D defaultCursor, hoverCursor;

    private void Awake()
    {
        //Init all the buttons in the menus
        playButton = GameObject.Find("Play Button").GetComponent<Button>();
        backButton = GameObject.Find("Back Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();

        playButton.onClick.AddListener(OpenLevels);
        backButton.onClick.AddListener(BackToMainMenu);
        quitButton.onClick.AddListener(QuitGame);

        //Get the level selection menu and the main menu so we can activate/deactivate as needed
        mainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu");
        levelMenu = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelMenu>();
    }

    //Opens the level select menu
    void OpenLevels()
    {
        ActivateMainMenu(false);
        levelMenu.ActivateLevelSelectMenu(true);
    }

    //Opens the main menu
    public void BackToMainMenu()
    {
        ActivateMainMenu(true);
        levelMenu.ActivateLevelSelectMenu(false);
    }

    //Helper to toggle the main menu
    public void ActivateMainMenu(bool maybe)
    {
        mainMenuCanvas.SetActive(maybe);
    }

    //Quits the game and brings you back to the desktop
    void QuitGame()
    {
        Application.Quit();
    }

    public void MouseHover()
    {
        //Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
    public void MouseEndHover()
    {
        //Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
    }
}

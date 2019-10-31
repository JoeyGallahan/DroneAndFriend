using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button playButton;
    Button quitButton;
    Button backButton;
    Button[] allLevelSelectionButtons = new Button[12];
    
    LevelController levelController;
    LevelDatabase levelDB;
    GameObject[] levelObjects;

    GameObject levelCanvas;
    GameObject mainMenuCanvas;       

    private void Awake()
    {
        //Init all the buttons in the menus
        playButton = GameObject.Find("Play Button").GetComponent<Button>();
        backButton = GameObject.Find("Back Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        levelObjects = GameObject.FindGameObjectsWithTag("LevelButton");
        levelDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelDatabase>();

        InitLevelButtons();

        playButton.onClick.AddListener(OpenLevels);
        backButton.onClick.AddListener(BackToMainMenu);
        quitButton.onClick.AddListener(QuitGame);

        //Get the level selection menu and the main menu so we can activate/deactivate as needed
        levelCanvas = GameObject.FindGameObjectWithTag("LevelList");
        mainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu");

        //Get the level controller so we can load and initialize the levels
        levelController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Disable the level select menu. We start from the main menu obviously
        ActivateLevelSelectMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Opens the level select menu
    void OpenLevels()
    {
        ActivateMainMenu(false);
        ActivateLevelSelectMenu(true);
    }

    //Opens the main menu
    void BackToMainMenu()
    {
        ActivateMainMenu(true);
        ActivateLevelSelectMenu(false);
    }

    //Helper to toggle the level select menu
    public void ActivateLevelSelectMenu(bool maybe)
    {
        levelCanvas.SetActive(maybe);
        if(maybe)
        {
            UpdateLevelButtons();
        }
    }

    //Helper to toggle the main menu
    void ActivateMainMenu(bool maybe)
    {
        mainMenuCanvas.SetActive(maybe);
    }

    //Sets up all the onClick listeners for each level select button
    void InitLevelButtons()
    {
        for (int i = 0; i < levelDB.levels.Count; i++)
        {
            //First, we setup all the buttons with what level they should load
            Button button = levelDB.levels[i].button.GetComponent<Button>();
            int levelNum = levelDB.levels[i].id;
            button.onClick.AddListener(delegate { LevelSelect(levelNum); });

            //Then, if they aren't unlocked yet, just disable them
            if (!levelDB.levels[i].unlocked)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    void UpdateLevelButtons()
    {
        for (int i = 0; i < levelDB.levels.Count; i++)
        {
            Button button = levelDB.levels[i].button.GetComponent<Button>();

            if (levelDB.levels[i].unlocked)
            {
                button.gameObject.SetActive(true);

                string fastestTime = levelDB.levels[i].fastestLevelTime.ToString("F2");
                string starsEarned = levelDB.levels[i].starsEarned.ToString();

                levelDB.levels[i].fastestTimeText.SetText(fastestTime);
                levelDB.levels[i].starsEarnedText.SetText(starsEarned);
            }
        }
    }

    //Gets the level that the player selected
    void LevelSelect(int levelNum)
    {
        //Find the level object in the db that the player selected
        for (int i = 0; i < levelDB.levels.Count; i++)
        {
            if (levelDB.levels[i].id == levelNum)
            {
                OpenLevel(levelDB.levels[i].level);
            }
        }
    }

    //Opens up the level and disables the menus
    void OpenLevel(GameObject level)
    {
        ActivateMainMenu(false);
        ActivateLevelSelectMenu(false);

        levelController.InitNewLevel(level);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}

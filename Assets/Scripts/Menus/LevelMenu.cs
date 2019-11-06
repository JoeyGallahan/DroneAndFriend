using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    Button[] allLevelSelectionButtons = new Button[12];

    LevelController levelController;
    LevelDatabase levelDB;
    GameObject[] levelObjects;

    GameObject levelCanvas;

    [SerializeField] Sprite starEarnedSprite;

    MainMenu mainMenu;

    private void Awake()
    {
        levelObjects = GameObject.FindGameObjectsWithTag("LevelButton");
        levelDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelDatabase>();

        levelCanvas = GameObject.FindGameObjectWithTag("LevelList");
        
        InitLevelButtons(); //Do the onclick listeners

        //Get the level controller so we can load and initialize the levels
        levelController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();

        mainMenu = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenu>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ActivateLevelSelectMenu(false); //Disable the level select menu. We start from the main menu
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

    //Updates the stats for each level based on the scores
    void UpdateLevelButtons()
    {
        //Goes through each level in the database
        for (int i = 0; i < levelDB.levels.Count; i++)
        {
            if (levelDB.levels[i].unlocked) //If this level has been unlocked
            {
                Button button = levelDB.levels[i].button.GetComponent<Button>(); //Gets this level's button in the level select menu 
                Image lockImage = GetLockImage(button); //Gets the image of the lock for the level
                if (lockImage) //If it exists (will exist for every level except the first)
                {
                    lockImage.enabled = false; //Disable it. It's unlocked
                }

                button.gameObject.SetActive(true); //Make the button show in the menu

                string fastestTime = levelDB.levels[i].fastestLevelTime.ToString("F2") + "s"; //Converts the fastest time into a string and makes it only 2-decimal places so that it doesn't get too long.
                levelDB.levels[i].fastestTimeText.SetText(fastestTime); //Sets the text of this button to show your fastest time earned

                int starsEarned = levelDB.levels[i].starsEarned; //Get the number of stars earned
                for (int s = 0; s < starsEarned; s++)
                {
                    levelDB.levels[i].starsEarnedImages[s].sprite = starEarnedSprite; //Updates the sprite(s) of the star(s) to show what you earned
                }
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

    //Helper to toggle the level select menu
    public void ActivateLevelSelectMenu(bool maybe)
    {
        levelCanvas.SetActive(maybe);
        if (maybe)
        {
            UpdateLevelButtons();
        }
    }

    //Opens up the level and disables the menus
    void OpenLevel(GameObject level)
    {
        mainMenu.ActivateMainMenu(false);
        ActivateLevelSelectMenu(false);

        levelController.InitNewLevel(level);
    }

    //Gets the image of the lock based off of the button
    //Basically goes through each parent until it finds the one with the tag we're looking for
    Image GetLockImage(Button btn)
    {
        Transform t = btn.transform;

        while(t.parent != null)
        {
            if (t.parent.tag.Equals("LevelWrapper"))
            {
                return t.parent.gameObject.GetComponent<Image>();
            }

            t = t.parent.transform;
        }

        return null;
    }

}

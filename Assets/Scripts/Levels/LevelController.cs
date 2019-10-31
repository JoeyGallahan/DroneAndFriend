using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    //Objects needed
    GameObject player;
    GameObject drone;
    GameObject startingPlatform;
    GameObject endingPlatform;
    Camera cam;

    //----UI things----//
    //HUD
    GameObject HUD;
    TextMeshProUGUI hudTimeText;
    //End-level scoring
    GameObject endScoreUI;
    TextMeshProUGUI endStarsText;
    TextMeshProUGUI endTimeText;
    Button restartLevelButton;
    Button returnButton;
    //MainMenu
    MainMenu mainMenu;
    //PauseMenu
    GameObject pauseMenu;
    Button pauseRestartButton;
    Button pauseReturnButton;

    //Colliders
    [SerializeField] BoxCollider2D endingPlatformCollider;
    [SerializeField] BoxCollider2D playerCollider;

    //Particles
    [SerializeField] ParticleSystem successParticles;

    //Level
    [SerializeField] Level currentLevel;
    bool levelActive = false;
    LevelDatabase levelDB;

    //Offsets
    Vector3 playerStartingOffset = new Vector3(0.5f, 0.5f);
    Vector3 droneStartingOffset = new Vector3(0.5f, 2.0f);

    float timeInLevel = 0.0f;
    bool paused = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();

        drone = GameObject.FindGameObjectWithTag("Drone");

        HUD = GameObject.FindGameObjectWithTag("HUD");
        hudTimeText = GameObject.FindGameObjectWithTag("TimeText").GetComponent<TextMeshProUGUI>();

        levelDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelDatabase>();

        endScoreUI = GameObject.FindGameObjectWithTag("EndScore");
        endStarsText = GameObject.FindGameObjectWithTag("EndStars").GetComponent<TextMeshProUGUI>();
        endTimeText = GameObject.FindGameObjectWithTag("EndTime").GetComponent<TextMeshProUGUI>();
        restartLevelButton = GameObject.FindGameObjectWithTag("EndRestartButton").GetComponent<Button>();
        returnButton = GameObject.FindGameObjectWithTag("EndReturnButton").GetComponent<Button>();

        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseRestartButton = GameObject.FindGameObjectWithTag("PauseRestartButton").GetComponent<Button>();
        pauseReturnButton = GameObject.FindGameObjectWithTag("PauseReturnButton").GetComponent<Button>();

        mainMenu = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenu>();

        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        drone.SetActive(false);
        player.SetActive(false);
        HUD.SetActive(false);
        endScoreUI.SetActive(false);
        pauseMenu.SetActive(false);

        restartLevelButton.onClick.AddListener(RestartLevel);
        returnButton.onClick.AddListener(ReturnToLevelSelect);
        
        pauseRestartButton.onClick.AddListener(RestartLevel);
        pauseReturnButton.onClick.AddListener(ReturnToLevelSelect);
    }

    private void Update()
    {
        //If you hit escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //If you are already paused and not in the end-level sequence or in the main menus or whatever
            if(paused && levelActive)
            {
                pauseMenu.SetActive(false); //close the pause menu
                paused = false; //youre no longer paused
                drone.GetComponent<DroneController>().CanMove(true); //the drone can move now
                player.GetComponent<RopeController>().CanUseRope(true); //the player can use the rope again
            }
            else if (!paused && levelActive) //If you're not already paused, but are currently in game
            {
                pauseMenu.SetActive(true); //open the pause menu
                paused = true; //the game is now paused
                drone.GetComponent<DroneController>().CanMove(false); //the drone cannot move
                player.GetComponent<RopeController>().CanUseRope(false); //the player can't use the rope or move
            }
        }
    }

    void LateUpdate()
    {
        if(levelActive && !paused)
        {
            UpdateTimeInLevel();
            CheckLevelEnd();
        }
    }

    //Initializes a new room
    public void InitNewLevel(GameObject room)
    {
        levelActive = true; //you are now currently playing a level
        paused = false;
        timeInLevel = 0.0f; //reset the time(just in case we're restarting)

        //Gets the appropriate level that we are initializing
        for(int i = 0; i < levelDB.levels.Count; i++)
        {
            if (levelDB.levels[i].level == room)
            {
                currentLevel = levelDB.levels[i];
                break;
            }
        }

        //Activates the player, drone, and level that we are loading into
        player.SetActive(true);
        drone.SetActive(true);

        currentLevel.level.SetActive(true);

        startingPlatform = GameObject.FindGameObjectWithTag("StartingPlatform"); //grab the starting platform so that we know where to put the player & drone

        //Grab the ending platform and its collider for the end level sequence
        endingPlatform = GameObject.FindGameObjectWithTag("EndingPlatform");
        endingPlatformCollider = endingPlatform.GetComponent<BoxCollider2D>();

        successParticles = endingPlatform.GetComponentInChildren<ParticleSystem>(); //set up the particle effects for the end level sequence

        //Update the positions of the camera, player, and drone to the appropriate place in the new level
        UpdateCameraPosition();
        UpdatePlayerPosition();
        UpdateDronePosition();        

        //If the drone is unable to move, let it move.
        if (!drone.GetComponent<DroneController>().CanMove())
        {
            drone.GetComponent<DroneController>().CanMove(true);
        }

        //If the player is unable to use the rope, let it use the rope. Prevents issues with the rb velocity when we use the if statement
        if (!player.GetComponent<RopeController>().CanUseRope())
        {
            player.GetComponent<RopeController>().CanUseRope(true);
        }
        player.GetComponent<RopeController>().ResetVelocity(); //makes it so they don't zoom away during a restart

        HUD.SetActive(true); //activate the HUD
    }

    //Moves the camera so that it focuses on the current level
    void UpdateCameraPosition()
    {
        Vector3 newCamPosition = currentLevel.level.transform.position;
        newCamPosition.z = cam.transform.position.z;

        cam.transform.position = newCamPosition;
    }

    //Moves the player to the starting platform
    void UpdatePlayerPosition()
    {
        Vector3 newPlayerPosition = startingPlatform.transform.position;
        newPlayerPosition += playerStartingOffset;

        player.transform.position = newPlayerPosition;
    }

    //Moves the drone to above the player
    void UpdateDronePosition()
    {
        Vector3 newDronePosition = startingPlatform.transform.position;
        newDronePosition += droneStartingOffset;

        drone.transform.position = newDronePosition;
    }

    //Checks to see if the player has collided with the ending platform
    private void CheckLevelEnd()
    {
        if (playerCollider.IsTouching(endingPlatformCollider) && timeInLevel > 0.1) 
        {
            EndLevel();
        }
    }

    //Performs the appropriate end-level scenario
    private void EndLevel()
    {
        levelActive = false; //no longer in the level
        paused = true; //the game is paused

        drone.GetComponent<DroneController>().CanMove(false); //the drone can't move
        player.GetComponent<RopeController>().CanUseRope(false); //the player can't use the rope and stops moving

        PlayParticles(); //Play the success particles
        ShowScore(); //updates and shows the score
    }

    //Plays the particle effects at the end of a level
    private void PlayParticles()
    {
        if (!successParticles.isPlaying)
        {
            successParticles.Play();
        }
    }

    //Calculates the number of stars earned using the time it took to complete the level
    private int CalculateStars()
    {
        for(int i = currentLevel.timeForStars.Length - 1; i > 0; i--)
        {
            if(timeInLevel <= currentLevel.timeForStars[i])
            {
                return i+1;
            }
        }

        return 1; 
    }

    //Shows the final time and stars earned for the level
    private void ShowScore()
    {
        int starsEarned = CalculateStars();
        
        endStarsText.SetText(starsEarned.ToString());
        endTimeText.SetText(timeInLevel.ToString("F2"));
        endScoreUI.SetActive(true);

        UpdateLevelScore(starsEarned);
    }

    //Updates the score in the leveldb so it will properly show later in the level select menu
    private void UpdateLevelScore(int stars)
    {
        //Only updates the stars if it's more than they've already earned
        if(stars > currentLevel.starsEarned)
        {
            currentLevel.starsEarned = stars;
        }

        //Only updates the time if it's less than their fastest time
        if(timeInLevel < currentLevel.fastestLevelTime || currentLevel.fastestLevelTime == 0.0f)
        {
            currentLevel.fastestLevelTime = timeInLevel;
        }

        int levelIndex = levelDB.levels.IndexOf(currentLevel);

        if (levelIndex < levelDB.levels.Count - 1)
        {
            if (currentLevel.starsEarned == 3)
            {
                levelDB.levels[levelIndex + 1].unlocked = true;
            }
        }

    }

    private void UpdateTimeInLevel()
    {
        timeInLevel += Time.deltaTime;

        hudTimeText.text = timeInLevel.ToString("F2");
    }

    private void RestartLevel()
    {
        //Update the positions of the player and drone to the appropriate place in the new level        
        UpdateDronePosition();
        UpdatePlayerPosition();
        
        endScoreUI.SetActive(false); //disables the end score screen
        pauseMenu.SetActive(false); //disables the pause menu

        timeInLevel = 0.0f; //reset the time
        paused = false; //you are no longer paused
        levelActive = true; //the level is reactivated

        drone.GetComponent<DroneController>().CanMove(true); //the drone can move again
        player.GetComponent<RopeController>().CanUseRope(true); //the player can use the rope again
        player.GetComponent<RopeController>().ResetVelocity(); //makes it so they don't zoom away during a restart
    }

    private void ReturnToLevelSelect()
    {
        timeInLevel = 0.0f; //reset the time

        endScoreUI.SetActive(false); //disables the end score screen
        currentLevel.level.SetActive(false); //disables the current level
        pauseMenu.SetActive(false); //disables the pause menu

        mainMenu.ActivateLevelSelectMenu(true); //opens the level select menu with updated scores and unlocked levels
    }
}

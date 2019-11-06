using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenu;
    Button pauseRestartButton;
    Button pauseReturnButton;

    bool paused = false;

    private void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseRestartButton = GameObject.FindGameObjectWithTag("PauseRestartButton").GetComponent<Button>();
        pauseReturnButton = GameObject.FindGameObjectWithTag("PauseReturnButton").GetComponent<Button>();
    }
    
    public bool IsPaused() { return paused; }

    public void TogglePause(bool maybe)
    {
        paused = maybe;
        pauseMenu.SetActive(maybe);
    }
}

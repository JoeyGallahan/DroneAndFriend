using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndMenu : MonoBehaviour
{
    //End-level scoring
    GameObject endScoreUI;
    [SerializeField] List<Image> endStars;
    TextMeshProUGUI endTimeText;
    [SerializeField] Sprite starEarnedSprite;
    [SerializeField] Sprite starDefaultSprite;

    //Buttons
    Button nextLevelButton;
    Button restartLevelButton;
    Button returnButton;

    private void Awake()
    {
        endScoreUI = GameObject.FindGameObjectWithTag("EndScore");
        endTimeText = GameObject.FindGameObjectWithTag("EndTime").GetComponent<TextMeshProUGUI>();
        nextLevelButton = GameObject.FindGameObjectWithTag("EndNextLevelButton").GetComponent<Button>();
        restartLevelButton = GameObject.FindGameObjectWithTag("EndRestartButton").GetComponent<Button>();
        returnButton = GameObject.FindGameObjectWithTag("EndReturnButton").GetComponent<Button>();
    }

    //Activates&Deactivates the end-level UI
    public void SetActive(bool maybe)
    {
        endScoreUI.SetActive(maybe);

        if (!maybe) //When you close the end score menu
        {
            for (int i = 0; i < 3; i++)
            {
                endStars[i].sprite = starDefaultSprite; //Change the stars back to the default sprite so that it won't carry over to the next level
            }

        }
    }

    //Updates the stars earned and time in level on the end-level UI
    public void UpdateScore(int starsEarned, float timeInLevel)
    {
        Debug.Log("Stars Earned: " + starsEarned);

        for (int i = 0; i < starsEarned; i++)
        {
            Debug.Log(i);
            endStars[i].sprite = starEarnedSprite; //Updates the sprite(s) of the star(s) to show what you earned
        }

        endTimeText.SetText(timeInLevel.ToString("F2")+ "s");

        SetActive(true);
    }

    //Makes it so the "Next Level Button" shows or hides. It changes in the level controller based on how many stars the player earns
    public void ToggleNextLevelButton(bool maybe)
    {
        nextLevelButton.gameObject.SetActive(maybe);
    }
}

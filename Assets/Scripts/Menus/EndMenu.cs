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
    Button nextLevelButton;
    Button restartLevelButton;
    Button returnButton;
    [SerializeField] Sprite starEarnedSprite;

    private void Awake()
    {
        endScoreUI = GameObject.FindGameObjectWithTag("EndScore");
        endTimeText = GameObject.FindGameObjectWithTag("EndTime").GetComponent<TextMeshProUGUI>();
        nextLevelButton = GameObject.FindGameObjectWithTag("EndNextLevelButton").GetComponent<Button>();
        restartLevelButton = GameObject.FindGameObjectWithTag("EndRestartButton").GetComponent<Button>();
        returnButton = GameObject.FindGameObjectWithTag("EndReturnButton").GetComponent<Button>();
    }

    public void SetActive(bool maybe)
    {
        endScoreUI.SetActive(maybe);
    }

    public void UpdateScore(int starsEarned, float timeInLevel)
    {
        for (int i = 0; i < starsEarned; i++)
        {
            endStars[i].sprite = starEarnedSprite; //Updates the sprite(s) of the star(s) to show what you earned
        }


        endTimeText.SetText(timeInLevel.ToString("F2")+ "s");
        SetActive(true);
    }

    public void ToggleNextLevelButton(bool maybe)
    {
        nextLevelButton.gameObject.SetActive(maybe);
    }
}

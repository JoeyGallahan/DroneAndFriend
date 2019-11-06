using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    [SerializeField] public GameObject level;
    [SerializeField] public GameObject button;
    [SerializeField] public List<Image> starsEarnedImages;
    [SerializeField] public TextMeshProUGUI fastestTimeText;
    [SerializeField] public int id;
    [SerializeField] public bool unlocked = false;
    [SerializeField] public float[] timeForStars;
    [SerializeField] public float fastestLevelTime = 0.0f;
    [SerializeField] public int starsEarned = 0;
}

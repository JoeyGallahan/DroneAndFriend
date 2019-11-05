using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDatabase : MonoBehaviour
{
    [SerializeField] public List<Level> levels = new List<Level>();

    public void SaveLevels()
    {
        SaveSystem.SaveLevels(this);
    }

    public void LoadLevels()
    {
        LevelData loadData = SaveSystem.LoadLevels();

        for(int i = 0; i < levels.Count; i++)
        {
            levels[i].unlocked = loadData.levelsUnlocked[i];
            levels[i].fastestLevelTime = loadData.fastestLevelTimes[i];
            levels[i].starsEarned = loadData.starsEarned[i];
        }
    }
}

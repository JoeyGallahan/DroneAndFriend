using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<bool>  levelsUnlocked = new List<bool>();
    public List<float> fastestLevelTimes = new List<float>();
    public List<int>   starsEarned = new List<int>();

    public LevelData(LevelDatabase db)
    {
        int ctr = 0;

        foreach(Level l in db.levels)
        {
            Debug.Log("Saving " + ctr + ": " + l.unlocked + ", " + l.fastestLevelTime + ", " + l.starsEarned);

            levelsUnlocked.Insert(ctr, l.unlocked);
            fastestLevelTimes.Insert(ctr, l.fastestLevelTime);
            starsEarned.Insert(ctr, l.starsEarned);
            
            ctr++;
        }
    }
}

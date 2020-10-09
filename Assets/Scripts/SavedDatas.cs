using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class SavedDatas
{
    [FormerlySerializedAs("currentLevel")] public static int currentSavedLevel;
    public int totalGemSaved;

    public int GetCurrentSavedLevel()
    {
        return currentSavedLevel;
    }

    public void SetCurrentLevel(int currentLevel)
    {
        currentSavedLevel = currentLevel;
        PlayerPrefs.SetInt("CURRENT_LEVEL",currentSavedLevel);
    }

    public void SetGem(int totalGem)
    {
        totalGemSaved = totalGem;
        PlayerPrefs.SetInt("GEM",totalGemSaved);
    }
}

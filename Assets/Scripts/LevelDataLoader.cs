using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataLoader : MonoBehaviour
{
    public static LevelDataLoader Instance;
    private int totalGem;
    private bool IsGame = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SingletonPattern();
        //Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        LoadGameDatas();
    }

    private void Update()
    {
        
        if (EventManager.Instance != null)
        {
            EventManager.Instance.NextLevelTriggered += GoNextLevel;
            EventManager.Instance.WallCollision += GoNextLevel;
        }
    }

    private void LoadGameDatas()
    {
        if (PlayerPrefs.HasKey("CURRENT_LEVEL"))
        {
            int currentLev = PlayerPrefs.GetInt("CURRENT_LEVEL");
            totalGem = PlayerPrefs.GetInt("GEM");
            LoadCurrentLevel(currentLev);
        }
       
        else
        {
            Debug.Log("current level 0 ");
            Debug.Log("no game saved");
            LoadFirstLevel();
        }
    }
    
    private void SingletonPattern()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void LoadCurrentLevel(int currentLevel)
    {
        SceneManager.LoadScene(currentLevel);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public int GetTotalGem()
    {
        return totalGem;
    }

    public void GoNextLevel()
    {
        PlayerPrefs.SetInt("GEM",GemManager.Instance.GetGemCount());
        totalGem = PlayerPrefs.GetInt("GEM");
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : BaseObject
{
    public static LevelManager Instance; 
    [SerializeField] private int currentLevel;
    [SerializeField] private Animator levelTransition;
    [SerializeField] private float transitionTime = 1f;
    public override void BaseObjectAwake()
    {

        SingletonPattern();
        EventManager.Instance.GoNextLevel += LoadNextLevel;
        EventManager.Instance.RestartLevel += RestartLevel;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        //GetSavedLevel();
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
    
    /*private void GetSavedLevel()
    {
        if (GameManager.Instance.GetSavedDatas() == null)
        {
            Debug.Log("first level");
            currentLevel = 1;
        }
        else
        { 
            currentLevel = GameManager.Instance.GetSavedDatas().GetCurrentSavedLevel();
            Debug.Log("current level = " + currentLevel);
        }
    }*/

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 
    
    private void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        levelTransition.SetTrigger("StartTransition");
        
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int currentLevel)
    {
        this.currentLevel = currentLevel;
    }
    
}

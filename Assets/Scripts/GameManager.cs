using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Main.BaseObject;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //variables
    public static GameManager Instance;
    [SerializeField] private BaseObject[] baseObjects;
    
    //GameObjects
    [SerializeField] private GameObject player;
    private GameObject endLine;
    
    private void Awake()
    {
        endLine = GameObject.Find("EndLine");
        SingletonPattern();
        CallBaseObjectAwakes();
        DeactiveScripts();
        EventManager.Instance.GameStarted += GameIsStarting;
        EventManager.Instance.WallCollision += CollidedWall;
        EventManager.Instance.NextLevelCollider += NextLevelCollided;
        EventManager.Instance.GoNextLevel += GoNextLevel;
        EventManager.Instance.Boxed += DeactiveScriptEnd;
        Application.targetFrameRate = 60;
    }
    
    private void Start()
    {
        CallBaseObjectStarts();
    }

    private void Update()
    {
        CallBaseObjectUpdates();
    }

    private void FixedUpdate()
    {
        CallBaseObjectFixedUpdates();
    }
    
    private void LateUpdate()
    {
        CallBaseObjectLateUpdates();
    }

    private void OnDestroy()
    {
        CallBaseObjectDestroys();
    }

    private void OnApplicationQuit()
    {
        CreateSaveGameObject();
        PlayerPrefs.Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        CreateSaveGameObject();
        PlayerPrefs.Save();
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
    
    private void CreateSaveGameObject()
    {
        SavedDatas save = new SavedDatas();
        save.SetCurrentLevel(LevelManager.Instance.GetCurrentLevel());
        save.SetGem(GemManager.Instance.GetGemCount());
    }
    
   
    
    private void CallBaseObjectAwakes()
    {
        for (int i = 0; i < baseObjects.Length; i++)
        {
            baseObjects[i].BaseObjectAwake();
        }
    }

    private void ActiveScripts()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<BrickSmash>().enabled = true;
    }
    
    private void DeactiveScripts()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<BrickSmash>().enabled = false;
    }

    private void ActiveScriptEnd()
    {
        player.GetComponent<EndOfLevel>().enabled = true;
    }
    
    private void DeactiveScriptEnd()
    {
        player.GetComponent<EndOfLevel>().enabled = false;
    }

    private void GameIsStarting()
    {
        ActiveScripts();
        TinySauce.OnGameStarted(SceneManager.GetActiveScene().buildIndex.ToString());
    }

    private void CollidedWall()
    {
        DeactiveScripts();
        TinySauce.OnGameFinished(SceneManager.GetActiveScene().buildIndex.ToString(),0);
    }
    private void NextLevelCollided()
    {
        DeactiveScripts();
        ActiveScriptEnd();
    }

    public void GoNextLevel()
    {
        Debug.Log("wdaskasd");
        TinySauce.OnGameFinished(SceneManager.GetActiveScene().buildIndex.ToString(),0);
    }

    private void CallBaseObjectStarts()
    {
        for (int i = 0; i < baseObjects.Length; i++)
        {
            baseObjects[i].BaseObjectStart();
        }
    }

    private void CallBaseObjectUpdates()
    {
        for (int i = 0; i < baseObjects.Length; i++)
        {
            baseObjects[i].BaseObjectUpdate();
        }
    }

    private void CallBaseObjectFixedUpdates()
    {
        for (int i = 0; i < baseObjects.Length; i++)
        {
            baseObjects[i].BaseObjectFixedUpdate();
        }
    }

    private void CallBaseObjectLateUpdates()
    {
        for (int i = 0; i < baseObjects.Length; i++)
        {
            baseObjects[i].BaseObjectLateUpdate();
        }
    }


    private void CallBaseObjectDestroys()
    {
        for (int i = 0; i < baseObjects.Length; i++)
        {
            baseObjects[i].BaseObjectDestroy();
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject GetEndLine()
    {
        return endLine;
    }
}

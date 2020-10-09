using System;
using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;

public class UIController : BaseObject
{
    public static UIController Instance;
    
    [SerializeField] private BaseObject[] Canvases;

    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Canvas marketCanvas;
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas deadCanvas;
    [SerializeField] private Canvas nextLevelCanvas;
    
    public override void BaseObjectAwake()
    {
        SingletonPattern();
        CallCanvasesAwakes();
        EventManager.Instance.GameStarted += GameStarted;
        EventManager.Instance.WallCollision += CollideWall;
        EventManager.Instance.NextLevelTriggered += NextLevelCollider;
    }
    
    public override void BaseObjectStart()
    {
        CallCanvasesStarts();
        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        marketCanvas.gameObject.SetActive(false);
        inGameCanvas.gameObject.SetActive(false);
        deadCanvas.gameObject.SetActive(false);
        nextLevelCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        CallCanvasesUpdates();
    }
    
    private void CallCanvasesAwakes()
    {
        for (int i = 0; i < Canvases.Length; i++)
        {
            Canvases[i].BaseObjectAwake();
        }
    }
    private void GameStarted()
    { 
        mainMenuCanvas.gameObject.SetActive(false);
        inGameCanvas.gameObject.SetActive(true);
    }
    private void CollideWall()
    {
        inGameCanvas.gameObject.SetActive(false);
        deadCanvas.gameObject.SetActive(true);
    }
    private void NextLevelCollider()
    {
        nextLevelCanvas.gameObject.SetActive(true);
    }
    
    private void CallCanvasesStarts()
    {
        for (int i = 0; i < Canvases.Length; i++)
        {
            Canvases[i].BaseObjectStart();
        }
    }
    
    private void CallCanvasesUpdates()
    {
        for (int i = 0; i < Canvases.Length; i++)
        {
            Canvases[i].BaseObjectUpdate();
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
    
    public Canvas GetMainMenuCanvas()
    {
        return mainMenuCanvas;
    }
    
    public Canvas GetSettingsCanvas()
    {
        return settingsCanvas;
    }
    
    public Canvas GetMarketCanvas()
    {
        return marketCanvas;
    }
}

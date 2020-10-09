using System;
using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;

public class EventManager : BaseObject
{
    

    public static EventManager Instance;
    
    //MainMenu Events
    public event Action SettingsButtonEnter;
    public event Action SettingsButtonExit;
    public event Action MarketButtonEnter;
    public event Action MarketButtonExit;
    public event Action GameStarted;
    public event Action RestartLevel;
    public event Action GemCollided;
    public event Action NextLevelCollider;
    public event Action NextLevelTriggered;
    public event Action Holding;
    public event Action Released;
    public event Action WallCollision;
    public event Action GoNextLevel;
    public event Action CollideObstacle;
    
    public event Action Boxed;

    public event Action PlayerSmashedWall;
    
    //InGame Events
    
    public override void BaseObjectAwake()
    {
        SingletonPattern();
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
    
    public void OnSettingsButtonEnter()
    {
        SettingsButtonEnter?.Invoke();
    }
    
    public void OnSettingsButtonExit()
    {
        SettingsButtonExit?.Invoke();
    }
    
    public void OnMarketButtonEnter()
    {
        MarketButtonEnter?.Invoke();
    }
    
    public void OnMarketButtonExit()
    {
        MarketButtonExit?.Invoke();
    }
    
    public void IsGameStart()
    {
        GameStarted?.Invoke();
    }

    public void IsRestartLevel()
    {
        RestartLevel?.Invoke();
    }

    public void IsGemCollided()
    {
        GemCollided?.Invoke();
    }

    public void IsCollideObstacle()
    {
        CollideObstacle?.Invoke();
    }

    public void IsBoxing()
    {
        Boxed?.Invoke();
    }
    
    public void WallCollider()
    {
        WallCollision?.Invoke();
    }

    public void OnHolding()
    {
        Holding?.Invoke();
    }

    public void OnReleased()
    {
        Released?.Invoke();
    }

    public void LevelTriggered()
    {
        NextLevelCollider?.Invoke();
    }

    public void NextLevelCanvasTriggered()
    {
        NextLevelTriggered?.Invoke();
    }

    public void GoNextLevelButtonPushed()
    {
        GoNextLevel?.Invoke();
    }

    public void OnPlayerSmashedWall()
    {
        PlayerSmashedWall?.Invoke();
    }
    
}

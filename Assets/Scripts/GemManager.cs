using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;
using UnityEngine.UI;

public class GemManager : BaseObject
{
    public static GemManager Instance; 
    [SerializeField] private Text gemCountText;
    private int gemCount;
    private int collectedInGame = 0;
    public override void BaseObjectAwake()
    {
        SingletonPattern();
        EventManager.Instance.GemCollided += AddGem;
        SetGem();
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

    public void MultiplyGem(int multiplier)
    {
        collectedInGame = collectedInGame * multiplier;
        gemCount = gemCount + collectedInGame;
        gemCountText.text = FormatNumber(gemCount);
    }

    public void AddGem()
    {
        gemCount++;
        collectedInGame++;
        gemCountText.text = FormatNumber(gemCount);
    }

    public void SetGem()
    {
        gemCount = LevelDataLoader.Instance.GetTotalGem();
        gemCountText.text = FormatNumber(gemCount);
    }

    public int GetGemCount()
    {
        return gemCount;
    }
    
     
    public  string FormatNumber(int num) {
       
        
        if (num >= 100000)
            return FormatNumber(num / 1000) + "k";
        else if (num >= 10000) {
            return (num / 1000D).ToString("0.#") + "k";
        }
        else if (num >= 1000)
        {
            return (num / 1000D).ToString("0.#") + "k";
        }
        return num.ToString("#,0");
    }
}

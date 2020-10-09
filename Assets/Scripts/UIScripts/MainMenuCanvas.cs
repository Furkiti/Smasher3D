using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MainMenuCanvas : BaseObject
{

    [SerializeField] private GameObject startButton;
    public override void BaseObjectAwake()
    {
        EventManager.Instance.SettingsButtonEnter += SettingsOpened;
        EventManager.Instance.MarketButtonEnter += MarketOpened;
        
    }
    

    public void SettingsOpened()
    {
        if (!UIController.Instance.GetMarketCanvas().gameObject.activeSelf)
        {
            UIController.Instance.GetSettingsCanvas().GetComponent<SettingsCanvas>().ShowCanvas();
            startButton.SetActive(false);
        }
    }
    
    public void MarketOpened()
    {
        if (!UIController.Instance.GetSettingsCanvas().gameObject.activeSelf)
        {
            UIController.Instance.GetMarketCanvas().GetComponent<MarketCanvas>().ShowCanvas();
            startButton.SetActive(false);
        }
    }

    
    public void ShowCanvas()
    {
        gameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
    }

    public GameObject GetStartButton()
    {
        return startButton;
    }
}

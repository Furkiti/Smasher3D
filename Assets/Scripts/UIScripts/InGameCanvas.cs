using System;
using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;

public class InGameCanvas : BaseObject
{
    public override void BaseObjectAwake()
    {
        EventManager.Instance.NextLevelCollider += NextLevelCollider;
    }

    private void NextLevelCollider()
    {
       
    }

    public void ShowCanvas()
    {
        gameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
    }
    
    
}

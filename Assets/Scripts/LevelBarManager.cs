using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Main.BaseObject;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBarManager : BaseObject
{

    private int currentLevel;
    [SerializeField] private Slider levelBar;
    [SerializeField] private Text currentLevelText;
    private Transform playerTransform;
    private Transform endLineTransform;
    private float maxDistance;
    
    public override void BaseObjectAwake()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform;
        endLineTransform = GameManager.Instance.GetEndLine().transform;
        maxDistance = GetDistance();
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        SetCurrentLevelText();

        //Debug.Log(playerTransform.position);
        //Debug.Log(endLineTransform.position);
    }

    private void SetCurrentLevelText()
    {
        currentLevelText.text = currentLevel.ToString();
    }

    public override void BaseObjectUpdate()
    {
        if (playerTransform.position.z <= maxDistance && playerTransform.position.z <= endLineTransform.position.z)
        {
            float distance = 1 - (GetDistance() / maxDistance);
            levelBar.value = distance;    
        }
        //levelBar.value += 0.1f;
    }

    private float GetDistance()
    {
        return endLineTransform.position.z - playerTransform.position.z;
    }
    
}

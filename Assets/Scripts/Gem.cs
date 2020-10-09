using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    { 
        EventManager.Instance.IsGemCollided();
        Destroy(gameObject);
    }
    
}

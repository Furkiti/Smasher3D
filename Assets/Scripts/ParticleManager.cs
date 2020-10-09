using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;

public class ParticleManager : BaseObject
{
    [SerializeField] private GameObject levelEndParticle;    
    public override void BaseObjectAwake()
    {
        EventManager.Instance.NextLevelTriggered += NextLevelCollided;
        levelEndParticle.GetComponent<ParticleSystem>().Play();
    }

    private void NextLevelCollided()
    {
        levelEndParticle.SetActive(true);
    }
}

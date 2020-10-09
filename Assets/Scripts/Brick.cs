using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    void Update()
    {
        //gameObject.SetActive(false);
       
        if (transform.position.y < -60 )
        {
            Destroy(gameObject);
        }
    }
}

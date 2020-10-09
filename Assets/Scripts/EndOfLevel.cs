using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField] private GameObject firstCamera;
    [SerializeField] private GameObject secondCamera;
    [SerializeField] private float speed;
    [SerializeField] private GameObject boxMaxch;

    [SerializeField] public float maxTouch;

    private bool isBoxed = false;

    private int touchCount = 0;
    public static EndOfLevel Instance;
    private void Awake()
    {
        SingletonPattern();
        boxMaxch = GameObject.Find("BoksTorbasi");
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

    public int GetTouchCount()
    {
        return touchCount;
    }

    // Update is called once per frame
    void Update()
    {
        firstCamera.gameObject.SetActive(false);
        secondCamera.gameObject.SetActive(true);
        if (isBoxed == false)
        {
            Time.timeScale = .7f;
        }
        
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            transform.localScale += new Vector3(.1f, .1f, .1f);
            boxMaxch.transform.localScale += new Vector3(.3f, .3f, 0f);
            touchCount++;
        }

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (touchCount < maxTouch)
                {
                    transform.localScale += new Vector3(.1f, .1f, .1f);
                    boxMaxch.transform.localScale += new Vector3(.3f, .3f, 0f);
                    touchCount++;
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.name.Equals("BoxingLine"))
        {
            GetComponent<Animator>().SetBool("IsBoxing",true);
            EventManager.Instance.IsBoxing();
            isBoxed = true;
            Time.timeScale = 1f;
            StartCoroutine(AnimationsController.Instance.ShakeEndCam(secondCamera)); ;
            speed = 0;
        }
        
    }
}

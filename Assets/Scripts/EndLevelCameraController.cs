using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class EndLevelCameraController : MonoBehaviour
{
    [FormerlySerializedAs("yuzuk")] public GameObject circle;    
    private bool isBoxing = false;
    private bool isLerping = false;

    private float touchCounting;

    private GameObject gemCollected;

    public float maxTouch;
    public float multiplier;

    public float score;

    private bool end = false;

    private bool flag = false;

    private void Awake()
    {
        EventManager.Instance.Boxed += Boxing;
        circle = GameObject.Find("Circle");
    }

    // Update is called once per frame
    void Update()
    {
        if(isBoxing == false)
        transform.position += new Vector3(0,0,.15f);
    }

    private void LateUpdate()
    {
        //if (isLerping == true)
        //{
        //    if (touchCounting > -1)
        //    {
        //        parent.transform.position += new Vector3(0,1 * (touchCounting+1),0);

        //        touchCounting--;
        //    }
        //}

        if (isLerping && !end)
        {
            if (touchCounting > -1)
            {
                circle.transform.localPosition += new Vector3(0, (Mathf.Clamp(touchCounting / maxTouch, 0f, 1f)) / multiplier, 0);
                //parent.transform.position = Vector3.up * (Mathf.Clamp(touchCounting / maxTouch, 0f, 1f)) * Time.deltaTime;

                if (circle.transform.localPosition.y > (31f * Mathf.Clamp(touchCounting / maxTouch, 0f, 1f)))
                {
                    end = true;
                }
            }
        }

        if (end)
        {
            //Debug.Log("ended");
            EventManager.Instance.NextLevelCanvasTriggered();

            setText();
        }
    }

    public void setText()
    {
        if (flag) return;

        score = circle.transform.localPosition.y - 1.5f;

        GemManager.Instance.MultiplyGem((int)Mathf.Round(score / 5.9f));

        gemCollected = GameObject.Find("CollectedCoin");

        gemCollected.GetComponent<Text>().text = Mathf.Round(score / 5.9f).ToString() + "X GEM COLLECTED";

        flag = true;
    }

    public void Boxing()
    {
        isBoxing = true;
        StartCoroutine(BoxingAct());
    }

    public IEnumerator BoxingAct()
    {
        yield return new WaitForSeconds(1.6f);

        transform.position = new Vector3(14, 17.4f, 506.78f);
        transform.SetParent(circle.transform);

        touchCounting = EndOfLevel.Instance.GetTouchCount();
        end = false;

        Debug.Log(touchCounting);

        ChangeCyclinder();
       
    }

    public void ChangeCyclinder()
    {
        //yield return new WaitForSeconds(.5f);
        /*int i = 0;
        while (i<5)
        {
            parent.transform.position += new Vector3(0,1,0);
            i++;
        }*/
        //int touchCounting = EndOfLevel.Instance.GetTouchCount();
        //parent.transform.position = Vector3.Lerp(transform.position,Vector3.up,Time.deltaTime);

        isLerping = true;
    }
  
}

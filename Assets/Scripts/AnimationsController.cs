using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class AnimationsController : BaseObject
{
    public static AnimationsController Instance;
    //MainMenu Animations
    [SerializeField] private GameObject settingsMenuContainer;
    [SerializeField] private float settingsMenuOpeningDuration;
    [SerializeField] private float settingsMenuClosingDuration;
    
    [SerializeField] private GameObject marketContainer;
    [SerializeField] private float marketOpeningDuration;
    [SerializeField] private float marketClosingDuration;

    [SerializeField] private GameObject brickSmashText;


    [SerializeField] private Camera playerCamera;
    private Vector3 playerCameraInitial;

    private GameObject box;
    private GameObject boxFront;
    
    
    public override void BaseObjectAwake()
    {
        SingletonPattern();
        DOTween.Init();
        EventManager.Instance.SettingsButtonEnter += OpenedSettingsButton;
        EventManager.Instance.SettingsButtonExit += ClosedSettingsButton;
        EventManager.Instance.MarketButtonEnter += OpenedMarketButton;
        EventManager.Instance.MarketButtonExit += ClosedMarketButton;
        EventManager.Instance.GameStarted += GameIsStarting;
        EventManager.Instance.NextLevelCollider += NextLevelCollider;
        EventManager.Instance.Holding += PlayerHolding;
        EventManager.Instance.Released += PlayerReleased;
        EventManager.Instance.WallCollision += WallCollider;
        EventManager.Instance.Boxed += Boxing;
        GetCameraInitialPos();
        box = GameObject.Find("Box");
        boxFront = GameObject.Find("BoxFront");
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
    
    private void GetCameraInitialPos()
    {
        playerCameraInitial = playerCamera.transform.localPosition;
    }


    public void OpenedSettingsButton()
    {
        if (settingsMenuContainer.gameObject.active)
        {
            settingsMenuContainer.gameObject.transform.DOScale(Vector3.one, settingsMenuOpeningDuration);
        }
        
    }
    
    public void ClosedSettingsButton()
    {
        settingsMenuContainer.gameObject.transform.DOScale(Vector3.zero, settingsMenuClosingDuration);
    }
    
    public void OpenedMarketButton()
    {
        if (marketContainer.gameObject.active)
        {
            marketContainer.gameObject.transform.DOScale(Vector3.one, marketOpeningDuration);
        }
        
    }
    
    public void ClosedMarketButton()
    {
        marketContainer.gameObject.transform.DOScale(Vector3.zero, marketClosingDuration);
    }

    public void GameIsStarting()
    {
        GameManager.Instance.GetPlayer().GetComponent<Animator>().SetBool("IsRunning",true);
        GameManager.Instance.GetPlayer().GetComponent<Animator>().SetLayerWeight(1,1);
    }

    public void NextLevelCollider()
    {
        GameManager.Instance.GetPlayer().GetComponent<Animator>().SetBool("IsNextLevel",true);
    }

    public void PlayerHolding()
    {
        GameManager.Instance.GetPlayer().GetComponent<Animator>().SetBool("IsSwelling",true);
    }

    public void PlayerReleased()
    {
        GameManager.Instance.GetPlayer().GetComponent<Animator>().SetBool("IsSwelling",false);
        //GameManager.Instance.GetPlayer().GetComponent<Animator>().SetBool("IsReleased",true);
    }

    private void WallCollider()
    { 
        
       GameManager.Instance.GetPlayer().GetComponent<Animator>().SetTrigger("CollideWall");
       GameManager.Instance.GetPlayer().GetComponent<Animator>().SetLayerWeight(1,0);
       
    }

    public void Boxing()
    {
        boxFront.GetComponent<Animator>().SetBool("IsBoxing",true);
        
        //box.GetComponent<Animator>().SetBool("IsBoxing",true);
       
    }
    
    /*public void GemCollided(GameObject gem)
    {
        Debug.Log("collide moruq");
        gem.transform.DOLocalMoveX(gemUICanvas.transform.position.x,0.7f).SetEase(Ease.OutSine).OnComplete(() => Destroy(gem.gameObject));
        gem.transform.DOMoveY(gemUICanvas.transform.position.y,1).SetEase(Ease.OutSine).OnComplete(() => Destroy(gem.gameObject));
        gem.transform.DOMoveZ(gemUICanvas.transform.position.z,1).SetEase(Ease.OutSine).OnComplete(() => Destroy(gem.gameObject));
    }*/

    
    public void BrickSmashText()
    {
        Sequence BrickSmashSeq = DOTween.Sequence();
        brickSmashText.SetActive(true);
        brickSmashText.transform.DOScale(Vector3.one, 1f);
        brickSmashText.transform.DOLocalMoveY(222f, 0.7f).SetEase(Ease.OutBounce);
        BrickSmashSeq.PrependInterval(0.7f);
        BrickSmashSeq.Append(brickSmashText.transform.DOScale(Vector3.zero, 1f)).OnComplete((() => brickSmashText.SetActive(false)));
        BrickSmashSeq.Append(brickSmashText.transform.DOLocalMoveY(143f, 0f));

    }

    public void playerCam()
    {
        //Debug.Log(playerCameraInitial);
        playerCamera.transform.DOShakePosition(1f, 1f).OnComplete((() => playerCamera.transform.localPosition = playerCameraInitial));
    }

    public IEnumerator ShakeEndCam(GameObject secondCam)
    {
        
        yield return new WaitForSeconds(.5f);
        secondCam.transform.DOShakePosition(1f, 1f);
    }

    public float GetSettingsMenuClosingDuration()
    {
        return settingsMenuClosingDuration;
    }
    
    public float GetMarketClosingDuration()
    {
        return marketClosingDuration;
    }
}

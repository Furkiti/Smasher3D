using System.Collections;
using System.Collections.Generic;
using Main.BaseObject;
using UnityEngine;

public class SettingsCanvas : BaseObject
{

    private GameObject startButton;
    private float hideTime;
    public override void BaseObjectAwake()
    {
        EventManager.Instance.SettingsButtonExit += SettingsClosed;
        startButton = UIController.Instance.GetMainMenuCanvas().GetComponent<MainMenuCanvas>().GetStartButton();
    }

    public override void BaseObjectStart()
    {
        hideTime = AnimationsController.Instance.GetSettingsMenuClosingDuration();
    }

    public void SettingsClosed()
    {
        StartCoroutine(HideCanvas());
    }

    public void ShowCanvas()
    {
        gameObject.SetActive(true);
    }

    public IEnumerator HideCanvas()
    {
        yield return new WaitForSeconds(hideTime);
        gameObject.SetActive(false);
        startButton.SetActive(true);
    }
    
}

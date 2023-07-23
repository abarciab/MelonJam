using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchController : MonoBehaviour
{
    [SerializeField] GameObject labParent, HUDparent, navButtons, workshop, CommandCenter, upButtons, continueButton;

    private void Start() {
        GameManager.i.OnDayEnd.AddListener(HideUI);
        GameManager.i.OnConfrontationComplete.AddListener(() => continueButton.SetActive(true));
    }

    void HideUI() {
        ShowHideUI(false);
    }

    void ShowHideUI(bool active) {
        labParent.SetActive(active);
        HUDparent.SetActive(active);
        continueButton.SetActive(active);
        navButtons.SetActive(active);
        workshop.SetActive(active);
        CommandCenter.SetActive(active);
        upButtons.SetActive(!active);
    }

    public void GoDown() {
        HUDparent.SetActive(true);
        navButtons.SetActive(true);
        upButtons.SetActive(false);

        continueButton.SetActive(false);
        Camera.main.GetComponent<cameraController>().GoDown();
        GameManager.i.OnGoDown.Invoke();
    }

}

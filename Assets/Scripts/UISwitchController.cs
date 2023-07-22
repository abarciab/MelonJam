using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchController : MonoBehaviour
{
    [SerializeField] GameObject labParent, HUDparent, continueButton;

    private void Start() {
        GameManager.i.OnDayEnd.AddListener(HideUI);
        GameManager.i.OnConfrontationComplete.AddListener(() => continueButton.SetActive(true));
    }

    void HideUI() {
        labParent.SetActive(false);
        HUDparent.SetActive(false);
        continueButton.SetActive(false);
    }

    public void GoDown() {
        labParent.SetActive(true);
        HUDparent.SetActive(true);
        continueButton.SetActive(false);
        Camera.main.GetComponent<cameraController>().GoDown();
        GameManager.i.OnGoDown.Invoke();
    }

}

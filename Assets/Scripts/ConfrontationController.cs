using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrontationController : MonoBehaviour
{
    [HideInInspector] public ConfrontationDisplay display;
    [SerializeField] int numAgents;

    [SerializeField] int dayResetTime = 2;
    [SerializeField] float susThreshold = 1;
    int dayCountdown;
    bool ready;

    private void Start() {
       
        GameManager.i.OnDayEnd.AddListener(Check);
    }

    void Check() {
        if (ready) StartConfrontation();
        else if (GameManager.i.suspicion > susThreshold) PrepareConfrontation();
    }

    void PrepareConfrontation() {
        ready = true;
        FindObjectOfType<EventController>().DontSendInspectorTomorrow();
    }

    void StartConfrontation() {
        ready = false;
        display.StartConfrontation(numAgents);
    }

}

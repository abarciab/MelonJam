using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrontationController : MonoBehaviour
{
    [HideInInspector] public ConfrontationDisplay display;
    [SerializeField] int numAgents;

    [SerializeField] int dayResetTime = 2;
    int dayCountdown;

    private void Start() {
        dayCountdown = dayResetTime;
        GameManager.i.OnDayEnd.AddListener(Tick);
    }

    void Tick() {
        dayCountdown -= 1;
        if (dayCountdown <= 0) StartConfrontation();
    }

    void StartConfrontation() {
        dayCountdown = dayResetTime;
        display.StartConfrontation(numAgents);
    }

}

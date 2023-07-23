using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    [Header("colors")]
    public Color infectedColor;
    public Color agroColor, mutatedColor, affordableColor, unaffordableColor;

    [Space()]
    [SerializeField] float minWaitTime;
    public float suspicion, maxSuspicion;

    public int currDay { get; private set; } = 0;
    [HideInInspector]public UnityEvent OnDayEnd = new UnityEvent(), OnConfrontationComplete = new UnityEvent(), OnGoDown = new UnityEvent();
    [HideInInspector] public LabController labCon;
    [HideInInspector] public ConfrontationController confrontationCon;
    [HideInInspector] public VirusController virus;
    [HideInInspector] public HeadlineCoordinator headline;
    [HideInInspector] public Ecosystem eco;
    [HideInInspector] public Map map;
    [HideInInspector] public Resource HoveredResource;

    public bool inCommandCenter;
    public bool aimingHook;

    public void IncreaseSuspicion(float amount) {
        suspicion += amount;
    }

    private void Awake() {
        i = this;
        virus = FindObjectOfType<VirusController>(true);
        labCon = FindObjectOfType<LabController>(true);
        eco = FindObjectOfType<Ecosystem>(true);
        headline = FindObjectOfType<HeadlineCoordinator>(true);
        confrontationCon = FindObjectOfType<ConfrontationController>(true);
        map = FindObjectOfType<Map>(true);
    }

    public void EndDay() {
        currDay += 1;
        OnDayEnd.Invoke();
        inCommandCenter = false;
        aimingHook = false;

        StartCoroutine(WaitThenContinue());
    }

    IEnumerator WaitThenContinue() {
        yield return new WaitForSeconds(minWaitTime);

        OnConfrontationComplete.Invoke();
    }

    public void MovePlayer(bool commandCenter) {
        inCommandCenter = commandCenter;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    public Color infectedColor;
    [SerializeField] float minWaitTime;

    public int currDay { get; private set; } = 0;
    [HideInInspector]public UnityEvent OnDayEnd = new UnityEvent(), OnConfrontationComplete = new UnityEvent(), OnGoDown = new UnityEvent();
    [HideInInspector] public LabController labCon;
    [HideInInspector] public ConfrontationController confrontationCon;
    [HideInInspector] public VirusController virus;
    [HideInInspector] public HeadlineCoordinator headline;
    [HideInInspector] public Ecosystem eco;
    [HideInInspector] public Map map;

    private void Awake() {
        i = this;
        virus = FindObjectOfType<VirusController>();
        labCon = FindObjectOfType<LabController>();
        eco = FindObjectOfType<Ecosystem>();
        headline = FindObjectOfType<HeadlineCoordinator>();
        confrontationCon = FindObjectOfType<ConfrontationController>();
        map = FindObjectOfType<Map>();
    }

    public void EndDay() {
        currDay += 1;
        OnDayEnd.Invoke();

        StartCoroutine(WaitThenContinue());
    }

    IEnumerator WaitThenContinue() {
        yield return new WaitForSeconds(minWaitTime);

        OnConfrontationComplete.Invoke();
    }
}

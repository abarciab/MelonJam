using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    public int currDay { get; private set; } = 0;

    [HideInInspector]public UnityEvent OnDayStart = new UnityEvent();
    [HideInInspector] public LabController labCon;
    [HideInInspector] public VirusController virus;

    private void Awake() {
        i = this;
        virus = FindObjectOfType<VirusController>();
        labCon = FindObjectOfType<LabController>();
    }

    public void EndDay() {
        currDay += 1;
        OnDayStart.Invoke();
    }
}

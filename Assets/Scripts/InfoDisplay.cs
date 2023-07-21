using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] string dayPrefix = "Day ";
    [SerializeField] TextMeshProUGUI dayDisplay;

    [Header("Ingredients")]
    [SerializeField] string saltPrefix = "Day ";
    [SerializeField] TextMeshProUGUI saltDisplay;
    GameManager gMan;
    LabController labCon;

    [Header("VirusStats")]
    [SerializeField] string salinityPrefix = "Salinity: ";
    [SerializeField] TextMeshProUGUI virusSalinity;

    private void Start() {
        gMan = GameManager.i;
        gMan.OnDayStart.AddListener(UpdateDisplay);

        labCon = gMan.labCon;
    }

    private void Update() {
        saltDisplay.text = saltPrefix + labCon.salt;
        virusSalinity.text = salinityPrefix + gMan.virus.salinity + "%";
    }

    public void UpdateDisplay() {
        dayDisplay.text = dayPrefix + gMan.currDay.ToString();
    }
}

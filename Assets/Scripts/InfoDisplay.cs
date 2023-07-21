using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] string dayPrefix = "Day ";
    [SerializeField] TextMeshProUGUI dayDisplay;

    [Header("Ingredients")]
    [SerializeField] Ingredient saltIngrd;
    [SerializeField] TextMeshProUGUI saltDisplay;
    GameManager gMan;
    LabController labCon;

    [Header("VirusStats")]
    [SerializeField] string salinityPrefix = "Salinity: ";
    [SerializeField] TextMeshProUGUI virusSalinity;

    [Space()]
    [SerializeField] TextMeshProUGUI currentReagent;

    private void Start() {
        gMan = GameManager.i;
        gMan.OnDayStart.AddListener(UpdateDisplay);

        labCon = gMan.labCon;
    }

    private void Update() {
        saltDisplay.text = saltIngrd.prefix + labCon.GetIngredientCount(saltIngrd);
        virusSalinity.text = salinityPrefix + gMan.virus.salinity + "%";
    }

    public void UpdateDisplay() {
        dayDisplay.text = dayPrefix + gMan.currDay.ToString();
    }
}

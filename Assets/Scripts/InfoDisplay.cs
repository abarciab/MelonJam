using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] string dayPrefix = "Day ";
    [SerializeField] TextMeshProUGUI dayDisplay;

    GameManager gMan;
    LabController labCon;

    [Space()]
    [SerializeField] TextMeshProUGUI currentReagent;

    private void Start() {
        gMan = GameManager.i;
        gMan.OnDayEnd.AddListener(UpdateDisplay);

        labCon = gMan.labCon;
    }

    private void Update() {
        currentReagent.text = labCon.curentReagent ? labCon.curentReagent.name : "";
    }

    public void UpdateDisplay() {
        dayDisplay.text = dayPrefix + gMan.currDay.ToString();
    }
}

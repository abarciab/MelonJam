using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] string dayPrefix = "Day ";
    [SerializeField] TextMeshProUGUI dayDisplay;

    GameManager gMan;
    LabController labCon;

    [Space()]
    [SerializeField] Image currentReagentIcon;
    [SerializeField] TextMeshProUGUI currentReagentName, currentReagentInfo;
    [SerializeField] GameObject currentReagentParent;

    private void Start() {
        gMan = GameManager.i;
        gMan.OnDayEnd.AddListener(UpdateDisplay);

        labCon = gMan.labCon;
    }

    private void Update() {
        currentReagentName.text = labCon.curentReagent ? labCon.curentReagent.name : "";
        currentReagentInfo.text = labCon.curentReagent ? labCon.curentReagent.info : "";
        currentReagentIcon.sprite = labCon.curentReagent ? labCon.curentReagent.icon : null;
        currentReagentParent.SetActive(labCon.curentReagent);
    }

    public void UpdateDisplay() {
        dayDisplay.text = dayPrefix + gMan.currDay.ToString();
    }
}

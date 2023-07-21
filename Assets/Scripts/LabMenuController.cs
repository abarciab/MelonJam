using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabMenuController : MonoBehaviour
{
    [Header("end day")]
    [SerializeField] GameObject endDayButton;
    [SerializeField] GameObject addReagentButton;

    [Header("make reagent")]
    [SerializeField] GameObject startCraftingButton;

    LabController labCon;

    private void Start() {
        labCon = GameManager.i.labCon;
    }

    private void Update() {
        startCraftingButton.SetActive(!labCon.hasReagent);

        endDayButton.SetActive(!labCon.hasReagent);
        addReagentButton.SetActive(labCon.hasReagent);
    }
}

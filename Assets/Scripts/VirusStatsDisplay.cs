using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VirusStatsDisplay : MonoBehaviour
{
    VirusController virus;

    [SerializeField] TextMeshProUGUI infectivityText, mutabilityText, lethalityText, agressionText;
    [SerializeField] string infectvityPrefix = "Infectivity: ", mutabilityPrefix = "Mutability: ", lethalityPrefix = "Lethality: ", agressionPrefix = "Agression: ";

    private void Start() {
        virus = GameManager.i.virus;
    }

    private void Update() {
        infectivityText.text = infectvityPrefix + virus.infectivity;
        mutabilityText.text = mutabilityPrefix + virus.mutability;
        lethalityText.text = lethalityPrefix + virus.lethality;
        agressionText.text = agressionPrefix + virus.aggression;
    }
}

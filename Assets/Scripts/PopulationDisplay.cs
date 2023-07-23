using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationDisplay : MonoBehaviour
{
    [SerializeField] Ecosystem.CreatureType type;
    [SerializeField] TextMeshProUGUI healthy, infected, mutated, agro;
    Ecosystem eco;

    private void Start() {
        eco = GameManager.i.eco;
    }

    private void Update() {

        int infectedCount = eco.CheckStatus(Creature.Status.infected, type);
        int mutatedCount = eco.CheckStatus(Creature.Status.mutated, type);
        int agroCount = eco.CheckStatus(Creature.Status.agro, type);
        infectedCount += mutatedCount + agroCount;

        healthy.text = "Healthy: " + eco.CheckStatus(Creature.Status.healthy, type);
        infected.text = "Infected: " + infectedCount;
        mutated.text = "Mutated: " + mutatedCount;
        agro.text = "Agressive: " + agroCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{

    [Range(0, 100)] public float infectivity;
    [Range(0, 100)] public float mutability;
    [Range(0, 100)] public float lethality;
    [Range(0, 100)] public float aggression;
    public List<Ecosystem.CreatureType> unlockedSpecies = new List<Ecosystem.CreatureType>();

    public void addReagent(Reagent reagent) {
        foreach (var effect in reagent.effects) {
            ProcessEffect(effect);
        }
    }

    public void Unlock(Ecosystem.CreatureType Species) {
        unlockedSpecies.Add(Species);
    }

    void ProcessEffect(Effect effect) {
        switch (effect.type) {
            case Effect.Type.mutability:
                mutability += effect.amount;
                lethality = Mathf.Max(0, lethality);
                break;
            case Effect.Type.lethality:
                lethality += effect.amount;
                lethality = Mathf.Max(0, lethality);
                break;
            case Effect.Type.infectivity:
                infectivity += effect.amount;
                lethality = Mathf.Max(0, lethality);
                break;
            case Effect.Type.agression:
                aggression += effect.amount;
                break;
        }
    }
}

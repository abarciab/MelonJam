using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    public float infectivity { get; private set; }
    public float mutability { get; private set; }
    public float lethality { get; private set; }

    public float aggression { get; private set; }

    public void addReagent(Reagent reagent) {
        foreach (var effect in reagent.effects) {
            ProcessEffect(effect);
        }
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

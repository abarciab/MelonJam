using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public enum Type {mutability, lethality, infectivity, agression}
    public Type type;
    public float amount;
}

[CreateAssetMenu(fileName = "new Reagent", menuName = "ScriptableObjects/Reagent", order = 1)]
public class Reagent : ScriptableObject
{
    public override bool Equals(object other) {
        return Equals(other as Ingredient);
    }

    public bool Equals(Ingredient other) {
        if (other == null) return false;

        return other.name == name;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public List<Effect> effects = new List<Effect>();
}

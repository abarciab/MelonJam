using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class Ingredient : ScriptableObject
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

    public string prefix;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    [System.Serializable]
    public class Recipe
    {
        [HideInInspector] public string name;
        public List<Ingredient> input;
        public string output;
        public int priority;
        public bool strict = true;
    }

    private void OnValidate() {
        foreach (var r in recipes) {
            string inputString = ": ";
            foreach (var i in r.input) inputString += i.name + ", ";
            r.name = "[" + r.priority + "] " + r.output + inputString;
        }
    }

    [SerializeField] List<Recipe> recipes = new List<Recipe>();

    public List<string> CheckIngredients(List<Ingredient> ingredients) {
        List<Recipe> validOptions = new List<Recipe>();

        foreach (var recipe in recipes) {
            bool valid = true;
            if (recipe.strict) foreach (var ingrd in ingredients) if (!recipe.input.Contains(ingrd))valid = false;
            foreach (var ingrd in recipe.input) {
                if (!ingredients.Contains(ingrd)) {
                    valid = false;
                    break;
                }
            }
            if (valid) validOptions.Add(recipe);
        }

        var sortedOutput = SortByPriority(validOptions);

        return sortedOutput;
    }

    List<string> SortByPriority(List<Recipe> input) {
        var sorted = new List<string>();

        while (input.Count > 0) {
            int highest = -1;
            Recipe best = null;
            foreach (var r in input) {
                if (r.priority > highest) {
                    highest = r.priority;
                    best = r;
                }
            }
            input.Remove(best);
            sorted.Insert(sorted.Count, best.output);
        }
        return sorted;
    }
}

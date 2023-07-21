using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LabController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float sucsessChance;

    Dictionary<Ingredient, int> resources = new Dictionary<Ingredient, int>();
    public bool hasReagent { get; private set; }
    
    GameManager gMan;
    RecipeBook recipes;

    public void AddIngredient(Ingredient ingrd) {
        if (resources.ContainsKey(ingrd)) resources[ingrd] += 1;
        else resources.Add(ingrd, 1);
    }

    public Dictionary<Ingredient, int> GetResources() {
        return resources;
    }

    private void Start() {
        recipes = FindAnyObjectByType<RecipeBook>();
        gMan = GameManager.i;
        gMan.OnDayStart.AddListener(onDayStart);
    }

    public int GetIngredientCount(Ingredient ingrd) {
        if (resources.ContainsKey(ingrd)) return resources[ingrd];
        else return 0;
    }

    void RemoveIngredient(List<Ingredient> ingrds) {
        foreach (var i in ingrds) RemoveIngredient(i);
    }

    void RemoveIngredient(Ingredient ingrd) {
        if (resources.ContainsKey(ingrd)) resources[ingrd] -= 1;
    }

    void onDayStart() {
        hasReagent = false;
    }

    public void AttemptCraft(List<Ingredient> selectedIngrds) {
        RemoveIngredient(selectedIngrds);

        var validOptions = recipes.CheckIngredients(selectedIngrds);
        string chosenOutput = validOptions[0];

        float roll = Random.Range(0.0f, 1);
        if (roll < sucsessChance) CraftSuccsess(chosenOutput);
        else CraftFail(validOptions[validOptions.Count-1]);        
    }

    void CraftFail(string output) {
        print("crafted " + output);
        print("Crafting fail");
    }

    void CraftSuccsess(string output) {
        print(output);
        hasReagent = true;
    }

    public void UseReagent() {
        hasReagent = false;
        gMan.virus.addReagent();
    }

}

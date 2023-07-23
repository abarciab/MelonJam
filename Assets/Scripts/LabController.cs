using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LabController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float sucsessChance;

    Dictionary<Ingredient, int> resources = new Dictionary<Ingredient, int>();
    public Reagent curentReagent { get; private set; }

    [SerializeField] List<Ingredient> allIngredients = new List<Ingredient>();
    bool addedStarting;

    [Space()]
    [SerializeField] Sound makeReagentSound;
    [SerializeField] Sound addReagentSound;
    
    GameManager gMan;
    RecipeBook recipes;

    public void AddIngredient(Ingredient ingrd) {
        if (resources.ContainsKey(ingrd)) resources[ingrd] += 1;
        else resources.Add(ingrd, 1);
    }

    public Dictionary<Ingredient, int> GetIngredients() {
        if (!addedStarting) Start();
        return resources;
    }

    public bool CanAfford(List<KeyValuePair<Ingredient, int>> costs) {
        foreach (var c in costs) {
            if (GetIngredientCount(c.Key) < c.Value) return false;
        }
        return true;
    }

    private void Start() {
        makeReagentSound = Instantiate(makeReagentSound);
        addReagentSound = Instantiate(addReagentSound);

        recipes = FindAnyObjectByType<RecipeBook>();
        gMan = GameManager.i;
        gMan.OnDayEnd.AddListener(onDayStart);
        foreach (var i in allIngredients) AddEmpty(i);
        addedStarting = true;
    }

    void AddEmpty(Ingredient ingrd) {
        if (resources.ContainsKey(ingrd)) return;
        resources.Add(ingrd, 0);
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
        curentReagent = null;
    }

    public void AttemptCraft(List<Ingredient> selectedIngrds) {
        makeReagentSound.Play();
        RemoveIngredient(selectedIngrds);

        var validOptions = recipes.CheckIngredients(selectedIngrds);
        Reagent chosenOutput = validOptions[0];

        float roll = Random.Range(0.0f, 1);
        if (roll < sucsessChance) CraftSuccsess(chosenOutput);
        else CraftFail(validOptions[validOptions.Count-1]);        
    }

    void CraftFail(Reagent output) {
        print("failed");
        curentReagent = output;
    }

    void CraftSuccsess(Reagent output) {
        curentReagent = output;
    }

    public void UseReagent() {
        addReagentSound.Play();
        gMan.virus.addReagent(curentReagent);
        curentReagent = null;
    }

}

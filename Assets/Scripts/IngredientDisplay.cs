using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngredientDisplay : MonoBehaviour
{
    [SerializeField] Transform listParent;
    [SerializeField] GameObject displayPrefab;

    LabController labCon;
    Dictionary<Ingredient, TextMeshProUGUI> ingredientTexts = new Dictionary<Ingredient, TextMeshProUGUI>();

    private void Start() {
        labCon = GameManager.i.labCon;   
    }

    private void Update() {
        var ingredientData = labCon.GetIngredients();

        foreach (var entry in ingredientData) {
            if (ingredientTexts.ContainsKey(entry.Key)) {
                ingredientTexts[entry.Key].text = entry.Key.prefix + entry.Value;
                ingredientTexts[entry.Key].gameObject.SetActive(entry.Value > 0);
            }
            else MakeNewText(entry);
        }
    }

    void MakeNewText(KeyValuePair<Ingredient, int> entry) {
        var newText = Instantiate(displayPrefab, listParent);
        newText.GetComponent<TextMeshProUGUI>().text = entry.Key.prefix + entry.Value;

        ingredientTexts.Add(entry.Key, newText.GetComponent<TextMeshProUGUI>());
    }
}

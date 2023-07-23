using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InfectionTargetSelectionCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI creatureNameText, quantityText, costText;
    [SerializeField] Image backing, creatureSprite;
    [SerializeField] Button buyButton;
    List<KeyValuePair<Ingredient, int>> costs;

    GameManager gMan;

    private void Start() {
        gMan = GameManager.i;
    }

    public void Init(string creatureName, int quantity, List<KeyValuePair<Ingredient, int>> _costs, Sprite creatureSprite) {
        creatureNameText.text = creatureName;
        quantityText.text = "+" + quantity;

        string costString = "";
        for (int i = 0; i < _costs.Count-1; i++) {
            costString += _costs[i].Value + " " + _costs[i].Key.name + "\n";
        }
        costString += _costs[_costs.Count - 1].Value + " " + _costs[_costs.Count - 1].Key.name;
        costText.text = costString;

        this.creatureSprite.sprite = creatureSprite;

        costs = _costs;
    }

    private void Update() {
        backing.color = gMan.labCon.CanAfford(costs) ? gMan.affordableColor : gMan.unaffordableColor;
        buyButton.enabled = backing.color == gMan.affordableColor;
    }
}

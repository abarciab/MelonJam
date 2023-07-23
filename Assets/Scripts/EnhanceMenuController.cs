using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EnhanceMenuController : MonoBehaviour
{
    [System.Serializable]
    public class EnhanceOptionData
    {
        public TextMeshProUGUI text;
        public Button unlockButton;
        public List<Cost> costs = new List<Cost>();

        [System.Serializable]
        public class Cost
        {
            public Ingredient ingredient;
            public int cost;
        }

        public List<KeyValuePair<Ingredient, int>> GetCosts() {
            var _costs = new List<KeyValuePair<Ingredient, int>>();
            foreach (var c in costs) {
                _costs.Add(new KeyValuePair<Ingredient, int>(c.ingredient, c.cost));
            }
            return _costs;
        }
    }

    [SerializeField] List<EnhanceOptionData> data;
    [SerializeField] Color afforableColor, unAffordableColor;

    private void Start() {
        foreach (var d in data) {
            string costString = "";
            for (int i = 0; i < d.costs.Count - 1; i++) {
                costString += d.costs[i].cost + " " + d.costs[i].ingredient.name + "\n";
            }
            costString += d.costs[d.costs.Count - 1].cost + " " + d.costs[d.costs.Count - 1].ingredient.name;
            d.text.text = costString;
        }
    }

    private void Update() {
        foreach (var d in data) {
            d.unlockButton.enabled = GameManager.i.labCon.CanAfford(d.GetCosts());
            d.unlockButton.GetComponent<Image>().color = d.unlockButton.enabled ? afforableColor : unAffordableColor;
        }
    }

    public void UnlockCreature(int species) {
        GameManager.i.virus.Unlock((Ecosystem.CreatureType) species);
    }
}

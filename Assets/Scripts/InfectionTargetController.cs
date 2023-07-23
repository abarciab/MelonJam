using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionTargetController : MonoBehaviour
{
    [System.Serializable]
    public class TargetCreatureData
    {
        public string creatureName;
        public int quantity;
        public List<Cost> costs = new List<Cost>();
        public Ecosystem.CreatureType creature;
        public Sprite creatureIcon;

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
    [SerializeField] List<TargetCreatureData> data = new List<TargetCreatureData>();
    [SerializeField] List<InfectionTargetSelectionCoordinator> coords = new List<InfectionTargetSelectionCoordinator>();

    private void OnEnable() {
        for (int i = 0; i < coords.Count; i++) {
            if (i >= data.Count) break;
            var d = data[i];
            coords[i].Init(d.creatureName, d.quantity, d.GetCosts(), d.creatureIcon);
        }
    }
}

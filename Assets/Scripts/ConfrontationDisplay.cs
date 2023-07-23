using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrontationDisplay : MonoBehaviour
{
    [SerializeField] Transform characterParent;
    [SerializeField] GameObject policemanPrefab, fishermanPrefab, scientistPrefab;

    public List<Transform> ActiveCops = new List<Transform>();
    List<Agent> AllCops = new List<Agent>();

    private void Start() {
        GameManager.i.confrontationCon.display = this;
    }

    public void StartConfrontation(int numAgents) {
        for (int i = 0; i < numAgents; i++) {
            var newAgent = Instantiate(policemanPrefab, characterParent);
            AllCops.Add(newAgent.GetComponent<Agent>());
        }
    }

    public void SendInspector(EventController.CharacterType type) {
        switch (type) {
            case EventController.CharacterType.fisherman:
                Instantiate(fishermanPrefab, characterParent);
                break;
            case EventController.CharacterType.scientist:
                Instantiate(scientistPrefab, characterParent);
                break;
        }
    }

    private void Update() {
        ActiveCops.Clear();
        foreach (var c in AllCops) if (c && c.InWater) ActiveCops.Add(c.transform);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrontationDisplay : MonoBehaviour
{
    [SerializeField] Transform characterParent;
    [SerializeField] GameObject policemanPrefab;

    private void Start() {
        GameManager.i.confrontationCon.display = this;
    }

    public void StartConfrontation(int numAgents) {
        for (int i = 0; i < numAgents; i++) {
            var newAgent = Instantiate(policemanPrefab, characterParent);
        }
    }
}

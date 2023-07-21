using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LabController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float sucsessChance;

    public bool hasReagent { get; private set; }
    public bool canCraft { get; private set; }
    public int salt { get; private set; } = 10;
    
    GameManager gMan;

    private void Start() {
        gMan = GameManager.i;
        gMan.OnDayStart.AddListener(onDayStart);
    }

    private void Update() {
        canCraft = salt > 0;
    }

    void onDayStart() {
        hasReagent = false;
        salt += 1;
    }

    public void AttemptCraft() {
        salt -= 1;

        float roll = Random.Range(0.0f, 1);
        if (roll < sucsessChance) CraftSuccsess();
        else CraftFail();        
    }

    void CraftFail() {
        print("Crafting fail");
    }

    void CraftSuccsess() {
        hasReagent = true;
    }

    public void UseReagent() {
        hasReagent = false;
        gMan.virus.addReagent();
    }

}

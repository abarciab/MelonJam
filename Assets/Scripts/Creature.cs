using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum Status { healthy, infected, mutated};

    Ecosystem.CreatureType type;
    Ecosystem eco;
    public Status status;
    GameManager gMan;
    protected SpriteRenderer sRend;

    public virtual void Init(Ecosystem _eco, Ecosystem.CreatureType _type, Status _status = Status.healthy) {
        eco = _eco;
        type = _type;
        status = _status;
        eco.AddStatusData(type, status);
    }

    protected virtual void Start() {
        sRend = GetComponent<SpriteRenderer>();
        gMan = GameManager.i;
        gMan.OnDayEnd.AddListener(Tick);
    }

    void Tick() {
        if (status == Status.healthy) CheckInfection();
    }

    void CheckInfection() {
        float roll = Random.Range(0.0f, 1);

        if (gMan.virus.infectivity / 100 > roll) ChangeStatus(Status.infected);
        if (status == Status.infected && sRend) sRend.color = gMan.infectedColor;
    }

    void ChangeStatus(Status newStatus) {
        eco.ChangeStatusData(type, newStatus, status);
        status = newStatus;
    }

    private void OnDestroy() {
        eco.RemoveStatusData(type, status);
    }
}

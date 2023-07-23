using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum Status { healthy, infected, mutated, agro};

    Ecosystem.CreatureType type;
    Ecosystem eco;
    public Status status;
    GameManager gMan;
    protected SpriteRenderer sRend;
    [SerializeField] protected Sprite mutatedSprite;
    [SerializeField] float mutateScaleIncrease = 1.5f;
    [SerializeField] GameObject mutattionParticles, agroTrail;
    Sprite originalSprite;

    public virtual void Attack(float damage) { }

    public virtual void OnWrapAround(bool top) {
        StopAllCoroutines();
        if (agroTrail && agroTrail.activeInHierarchy) StartCoroutine(StopThenStartTrail());
    }

    IEnumerator StopThenStartTrail() {
        agroTrail.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        agroTrail.SetActive(true);
    }

    public virtual void Init(Ecosystem _eco, Ecosystem.CreatureType _type, Status _status = Status.healthy) {
        eco = _eco;
        type = _type;
        status = _status;
        eco.AddStatusData(type, status);
    }

    protected virtual void Start() {
        sRend = GetComponent<SpriteRenderer>();
        if (sRend) originalSprite = sRend.sprite;
        gMan = GameManager.i;
        gMan.OnDayEnd.AddListener(Tick);
    }

    void Tick() {
        if (!GameManager.i.virus.unlockedSpecies.Contains(type)) return;

        if (status == Status.healthy) CheckInfection();
        if (status == Status.infected) CheckMutation();
        if (status == Status.mutated) {
            CheckAgro();
            if (status == Status.agro) transform.localScale /= mutateScaleIncrease;
        }
        if (status == Status.infected) CheckAgro();
        if (status != Status.healthy) CheckLethality();
    }

    void CheckLethality() {
        float roll = Random.Range(0.0f, 1);
        if (gMan.virus.lethality / 100 > roll) ChangeStatus(Status.healthy);
        if (status != Status.healthy) return;

        sRend.color = Color.white;
        sRend.sprite = originalSprite;
        if (agroTrail) agroTrail.SetActive(false);
        if (mutattionParticles) mutattionParticles.SetActive(false);
    }

    void CheckAgro() {
        float roll = Random.Range(0.0f, 1);
        if (gMan.virus.aggression / 100 > roll) ChangeStatus(Status.agro);
        if (status != Status.agro) return;

        if (sRend) sRend.color = gMan.agroColor;
    }

    void CheckMutation() {
        float roll = Random.Range(0.0f, 1);

        if (gMan.virus.mutability / 100 > roll) ChangeStatus(Status.mutated);
        if (status != Status.mutated) return;

        if (sRend) sRend.sprite = mutatedSprite;
        if (sRend) sRend.color = gMan.mutatedColor;
        transform.localScale *= mutateScaleIncrease;
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

    protected virtual void Update() {
        if (mutattionParticles) mutattionParticles.SetActive(status == Status.mutated);
        if (agroTrail) agroTrail.SetActive(status == Status.agro);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMan : Agent
{
    [SerializeField] float inspectTime = 2;
    float inspectCooldown;
    bool atShore;
    List<Vector3> patrolPositions = new List<Vector3>();

    [SerializeField] float health, attackDamage, attackResetTime;
    float maxHealth, attackCooldown;


    protected override void Start() {
        base.Start();
        MoveTo(map.shore.position);
        inspectCooldown = inspectTime;
        patrolPositions = map.GetPatrolPositions();
        maxHealth = health;
    }

    protected override void Update() {
        base.Update();
        if (!moving) {
            inspectCooldown -= Time.deltaTime;
            if (inspectCooldown <= 0) {
                MoveToNextPoint();
            }
        }
        attackCooldown -= Time.deltaTime;
    }

    public float GetHPPercent() {
        return (health / maxHealth);
    }

    void MoveToNextPoint() {
        InWater = patrolPositions.Count > 0;

        if (patrolPositions.Count > 0) {
            atShore = false;
            MoveTo(patrolPositions[0]); 
            patrolPositions.RemoveAt(0);
            inspectCooldown = inspectTime;
        }
        else if (!atShore){
            MoveTo(map.shore.position);
        }
        else {
            MoveTo(map.station.position);
        }
    }

    protected override void OnReachDestination() {
        base.OnReachDestination();
        if (targetPos == map.shore.position) atShore = true;

        else if (targetPos == map.station.position) Destroy(gameObject);
    }

    public void Attack(float damage) {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!InWater) return;

        TryAttack(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!InWater) return;

        TryAttack(collision);
    }

    void TryAttack(Collider2D obj) {
        if (attackCooldown >= 0) return;
        var creature = obj.GetComponent<Creature>();
        if (creature && creature.status == Creature.Status.agro) {
            creature.Attack(attackDamage);
            attackCooldown = attackResetTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMan : Agent
{
    [SerializeField] float inspectTime = 2;
    float inspectCooldown;
    bool atShore;
    List<Vector3> patrolPositions = new List<Vector3>();

    [Header("Combat")]
    [SerializeField] float health;
    [SerializeField] float attackDamage, attackResetTime, knockBack, iFrameTime, normalGravity, bigGravity, dashForce, dashResetTime;
    float maxHealth, attackCooldown, iframeCooldown, dashCooldown;
    bool inCombat, fought;
    Rigidbody2D rb;

    protected override void Start() {
        base.Start();
        MoveTo(map.shore.position);
        patrolPositions = map.GetPatrolPositions();
        health *= GameManager.i.policeDifficulty;
        maxHealth = health;
        rb = GetComponent<Rigidbody2D>();
        normalGravity = rb.gravityScale;
    }

    protected override void Update() {
        attackCooldown -= Time.deltaTime;
        iframeCooldown -= Time.deltaTime;
        dashCooldown -= Time.deltaTime;

        if (iframeCooldown > 0) return;

        if (inCombat && GameManager.i.eco.CheckOnlyStatus(Creature.Status.agro) == 0) {
            inCombat = false;
            fought = true;
        }

        base.Update();
        if (inCombat) {
            moving = false;
            DoCombat();
            return;
        }

        if (!moving) {

            inspectCooldown -= Time.deltaTime;
            if (inspectCooldown <= 0) {
                MoveToNextPoint();
            }
        }
    }

    void DoCombat() {
        rb.gravityScale = map.waterline.position.y < transform.position.y ? bigGravity : normalGravity;

        if (dashCooldown <= 0) Dash();
    }

    void Dash() {
        dashCooldown = dashResetTime;
        var dir = (GameManager.i.map.waterCenter.position - transform.position).normalized;
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
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

        else if (targetPos == map.station.position) {
            if (fought) GameManager.i.CopWon();
            else GameManager.i.CopSurvived();
            Destroy(gameObject);
        }
    }

    //this is called when someone attacks this guy
    public void Attack(float damage, Vector2 sourcePos) {
        if (iframeCooldown > 0) return;
        health -= damage;
        if (health <= 0) {
            GameManager.i.CopDied();
            Destroy(gameObject);
        }
        inCombat = true;

        iframeCooldown = iFrameTime;

        KnockBack(sourcePos);
        
    }

    void KnockBack(Vector2 sourcePos) {
        var dir = (transform.position - (Vector3)sourcePos).normalized;
        GetComponent<Rigidbody2D>().AddForce(dir * knockBack, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!InWater) return;

        TryAttack(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!InWater) return;

        TryAttack(collision);
    }

    //this is when this guy tries to attack others
    void TryAttack(Collider2D obj) {
        if (attackCooldown >= 0) return;
        var creature = obj.GetComponent<Creature>();
        if (creature && creature.status == Creature.Status.agro) {
            creature.Attack(attackDamage * GameManager.i.policeDifficulty);
            attackCooldown = attackResetTime;
        }
    }
}

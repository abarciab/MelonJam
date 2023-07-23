using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumFish : Creature
{
    [SerializeField] Vector2 ampRange, periodRange, scaleRange, speedRange;
    [SerializeField] float amplitude, Period, speed;
    [SerializeField] float wrapAroundPunishment;
    bool mutated;
    ConfrontationDisplay display;

    [Header("agro")]
    [SerializeField] float chaseSpeed;
    [SerializeField] float attackDamage, attackResetTime, health;
    float attackCooldown, maxHealth;
    
    public float GetHPPercent() {
        return health / maxHealth;
    }

    protected override void Start() {
        base.Start();
        amplitude = Random.Range(ampRange.x, ampRange.y);
        Period = Random.Range(periodRange.x, periodRange.y);
        transform.localScale += Vector3.one * Random.Range(scaleRange.x, scaleRange.y);
        speed = Random.Range(speedRange.x, speedRange.y);

        if (Random.Range(0.0f, 1) > 0.5f) {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            speed *= -1;
        }
        else transform.localEulerAngles = new Vector3(0, 0, 0);

        display = GameManager.i.confrontationCon.display;
        maxHealth = health;
    }

    protected override void Update() {
        base.Update();
        if (status == Status.agro && display.ActiveCops.Count > 0) {
            AgroBehavior();
            return;
        }

        Vector3 posDelta = new Vector3(speed * Time.deltaTime, Mathf.Sin(Time.time * Period) * amplitude * Time.deltaTime, 0);
        transform.position += posDelta;
        transform.right = Vector3.Lerp(transform.right, posDelta.normalized, 0.05f);

        if (!mutated && status == Status.mutated) {
            mutated = true;
            GetComponent<Animator>().enabled = false;
            sRend.sprite = mutatedSprite;
        }
    }

    void AgroBehavior() {
        attackCooldown -= Time.deltaTime;

        AttackCops();
    }

    void AttackCops() {
        var activeCops = display.ActiveCops;

        var closestCop = activeCops[0];
        float closestDist = Mathf.Infinity;
        foreach (var c in activeCops) {
            float dist = Vector2.Distance(c.position, transform.position);
            if (dist < closestDist) {
                closestDist = dist;
                closestCop = c;
            }
        }

        MoveTo(closestCop);
    }

    void MoveTo(Transform target) {
        var dir = (target.position - transform.position).normalized;
        transform.position += dir * chaseSpeed * Time.deltaTime;
    }

    public override void OnWrapAround(bool top) {
        base.OnWrapAround(top);
        transform.position += Vector3.down * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (status != Status.agro) return;
        TryAttack(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (status != Status.agro) return;
        TryAttack(collision);
    }

    void TryAttack(Collider2D obj) {
        if (attackCooldown >= 0) return;
        var police = obj.GetComponent<PoliceMan>();
        if (police) {
            police.Attack(attackDamage);
            attackCooldown = attackResetTime;
        }
    }

    public override void Attack(float damage) {
        base.Attack(damage);
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}

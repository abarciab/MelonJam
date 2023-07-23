using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicPipe : Resource
{
    [SerializeField, Range(0, 1)] float collectibleChance;
    [SerializeField] Vector2 amountRange = new Vector2(1, 6);
    [SerializeField] GameObject particles;

    protected override void Start() {
        base.Start();
        collectible = false;
        GameManager.i.OnGoDown.AddListener(Roll);
    }

    void Roll() {
        float roll = Random.Range(0.0f, 1);
        if (roll < collectibleChance) {
            amount = Mathf.RoundToInt(Random.Range(amountRange.x, amountRange.y));
            collectible = true;
            particles.SetActive(true);
        }
        else {
            collectible = false;
            particles.SetActive(false);
        }
    }

    public override void Harvest() {
        base.Harvest();
        particles.SetActive(false);
    }
}

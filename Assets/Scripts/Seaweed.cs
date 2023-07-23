using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : Resource
{
    int maxGrowth;
    [SerializeField] int growthStage = 3;
    [SerializeField] Vector2 minMaxSize;

    public override void Harvest() {
        base.Harvest();

        growthStage -= 1;
        growthStage = Mathf.Max(0, growthStage);
        if (growthStage > 0) collectible = true;
    }

    protected override void Start() {
        base.Start();
        maxGrowth = growthStage;
        GameManager.i.OnGoDown.AddListener(Grow);
    }

    void Grow() {
        if (growthStage < minMaxSize.y) growthStage += 1;
    }

    private void Update() {
        Vector3 targetScale = new Vector3(transform.localScale.x, Mathf.Lerp(minMaxSize.x, minMaxSize.y, (float)growthStage / maxGrowth), 1);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.05f);
    }

}

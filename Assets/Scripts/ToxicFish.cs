using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFish : Resource
{
    [SerializeField] Vector2 speedRange = new Vector2(1, 3);
    float speed;

    protected override void Start() {
        base.Start();
        speed = Random.Range(speedRange.x, speedRange.y);
    }

    private void Update() {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    protected override void OnMouseDown() {
        base.OnMouseDown();
        Destroy(gameObject);
    }
}

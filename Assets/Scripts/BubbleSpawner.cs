using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] Vector2 timeRange = new Vector2(1, 4);
    [SerializeField] GameObject prefab;
    float cooldown;

    private void Start() {
        cooldown = Random.Range(timeRange.x, timeRange.y);
    }

    private void Update() {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0) {
            cooldown = Random.Range(timeRange.x, timeRange.y);
            Instantiate(prefab, transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFishController : MonoBehaviour
{
    [SerializeField] Vector2 bounds;
    [SerializeField] float maxFish;
    [SerializeField, Range(0, 1)] float spawnChance;
    [SerializeField] GameObject fishPrefab;

    private void Start() {
        GameManager.i.OnGoDown.AddListener(SpawnFishes);
        GameManager.i.OnConfrontationComplete.AddListener(KillFishes);
    }

    void KillFishes() {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void SpawnFishes() {
        for (int i = 0; i < maxFish; i++) {
            if (Random.Range(0.0f, 1) < spawnChance) SpawnFish();
        }
    }

    void SpawnFish() {
        var newFish = Instantiate(fishPrefab, transform);
        newFish.transform.position += new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y));
        Destroy(newFish, 25);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, bounds * 2);
    }
}

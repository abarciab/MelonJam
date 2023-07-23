using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour
{
    [SerializeField] GameObject obj1, obj2;

    private void Start() {
        obj1.SetActive(Random.Range(0.0f, 1) > 0.5f);
        obj2.SetActive(!obj1.activeInHierarchy);
    }
}

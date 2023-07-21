using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] Ingredient ingrd;
    [SerializeField] int amount;
    [SerializeField] Color fullColor, emptyColor;

    bool collectible;
    SpriteRenderer sRend;

    private void Start() {
        GameManager.i.OnDayStart.AddListener(ResetResource);
        sRend = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        sRend.color = collectible ? fullColor : emptyColor;
    }

    void ResetResource() {
        collectible = true;
    }

    private void OnMouseDown() {
        if (!collectible) return;
        collectible = false;

        GameManager.i.labCon.AddIngredient(ingrd);
    }
}

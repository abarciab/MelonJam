using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] Ingredient ingrd;
    [SerializeField] int amount;
    [SerializeField] Color fullColor, emptyColor;

    protected bool collectible;
    SpriteRenderer sRend;

    protected virtual void Start() {
        GameManager.i.OnDayEnd.AddListener(ResetResource);
        sRend = GetComponent<SpriteRenderer>();
        collectible = true;
    }

    private void Update() {
        sRend.color = collectible ? fullColor : emptyColor;
    }

    void ResetResource() {
        collectible = true;
    }

    protected virtual void OnMouseDown() {
        if (!collectible) return;
        collectible = false;

        GameManager.i.labCon.AddIngredient(ingrd);
    }
}

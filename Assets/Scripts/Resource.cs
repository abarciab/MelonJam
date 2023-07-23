using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] Ingredient ingrd;
    [SerializeField] protected int amount;
    [SerializeField] Color fullColor, emptyColor;

    protected bool collectible;
    SpriteRenderer sRend;
    [SerializeField] Sound harvestSound;

    protected virtual void Start() {
        GameManager.i.OnDayEnd.AddListener(ResetResource);
        sRend = GetComponent<SpriteRenderer>();
        collectible = true;
        if (harvestSound) harvestSound = Instantiate(harvestSound);
    }

    private void Update() {
        sRend.color = collectible ? fullColor : emptyColor;
    }

    void ResetResource() {
        collectible = true;
    }

    public virtual void Harvest() {
        if (!collectible) return;
        if (harvestSound) harvestSound.Play();
        collectible = false;
        for (int i = 0; i < amount; i++) {
            GameManager.i.labCon.AddIngredient(ingrd);
        }
    }

    private void OnMouseEnter() {
        GameManager.i.HoveredResource = this;
    }

    private void OnMouseExit() {
        if (GameManager.i.HoveredResource == this) GameManager.i.HoveredResource = null;
    }
}

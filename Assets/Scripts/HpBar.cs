using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] Slider hbBar;
    [SerializeField] PoliceMan cop;
    [SerializeField] MediumFish medFish;

    private void Update() {
        if (cop) hbBar.value = cop.GetHPPercent();
        if (medFish) hbBar.value = medFish.GetHPPercent();

        if (medFish) transform.GetChild(0).gameObject.SetActive(medFish.status == Creature.Status.agro);
    }
 }

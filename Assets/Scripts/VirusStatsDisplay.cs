using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class VirusStatsDisplay : MonoBehaviour
{
    VirusController virus;
    [SerializeField] Slider InfectiySlider, mutabilitySlider, lethalitySlider, agressionSlider;
    [SerializeField] TextMeshProUGUI infMod, lethMod, mutMod, agroMod;
    float oldInf, oldLeth, oldMut, oldAgro;

    [SerializeField] string positiveText, negativeText;
    
    private void Start() {
        virus = GameManager.i.virus;
    }

    private void Update() {
        if (oldInf != virus.infectivity) ShowMod(infMod, oldInf < virus.infectivity);
        if (oldLeth != virus.lethality) ShowMod(lethMod, oldLeth < virus.lethality);
        if (oldMut != virus.mutability) ShowMod(mutMod, oldMut < virus.mutability);
        if (oldAgro != virus.aggression) ShowMod(agroMod, oldAgro < virus.aggression);

        InfectiySlider.value = virus.infectivity/100;
        mutabilitySlider.value = virus.mutability/100;
        lethalitySlider.value = virus.lethality/100;
        agressionSlider.value = virus.aggression/100;

        oldInf = virus.infectivity;
        oldLeth = virus.lethality;
        oldMut = virus.mutability;
        oldAgro = virus.aggression;
    }

    void ShowMod(TextMeshProUGUI mod, bool Increased) {
        mod.text = Increased ? positiveText : negativeText;
        mod.gameObject.SetActive(true);
    }
}

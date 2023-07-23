using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SuspicionMeter : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Update() {
        slider.value = GameManager.i.suspicion / GameManager.i.maxSuspicion;
    }
}

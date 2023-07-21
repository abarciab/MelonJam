using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    public float salinity { get; private set; }

    public void addReagent() {
        salinity += 1;
    }
}

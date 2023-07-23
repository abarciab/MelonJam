using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] Sound music1;

    private void Start() {
        music1 = Instantiate(music1);
        music1.Play();
    }
}

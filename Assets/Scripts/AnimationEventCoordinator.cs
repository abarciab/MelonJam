using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCoordinator : MonoBehaviour
{
    public void Delete() {
        Destroy(gameObject);
    }

    public void Disable() {
        gameObject.SetActive(false);

    }
}

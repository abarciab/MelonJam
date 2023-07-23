using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float speed, amp = 0.1f, period = 1, lifeTime = 4;
    float offset;

    private void Start() {
        Destroy(gameObject, lifeTime);
        offset = Random.Range(-Mathf.PI * 2, Mathf.PI * 2);
    }

    void Update()
    {
        float x = Mathf.Sin(((Time.time + offset) % (2*Mathf.PI)) * period) * amp * Time.deltaTime;
        transform.position += new Vector3(x, speed * Time.deltaTime);
    }
}

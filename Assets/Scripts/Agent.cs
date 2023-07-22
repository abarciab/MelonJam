using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    Map map;
    [SerializeField] float inspectTime = 2, speed;
    float inspectCooldown;
    Vector3 targetPos;
    bool moving, atShore;

    private void Start() {
        map = GameManager.i.map;
        MoveTo(map.shore.position);
        inspectCooldown = inspectTime;
    }

    void MoveTo(Vector3 position) {
        targetPos = position;
        moving = true;
    }

    private void Update() {
        if (moving) {
            var dir = targetPos - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
            if (Vector2.Distance(transform.position, targetPos) < 0.1f) {
                moving = false;
                if (targetPos == map.shore.position) atShore = true;
                else Destroy(gameObject);
            }
        }
        else if (atShore) {
            inspectCooldown -= Time.deltaTime;
            if (inspectCooldown <= 0) MoveTo(map.station.position);
        }
    }
}

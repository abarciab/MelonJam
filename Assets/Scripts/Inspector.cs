using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : Agent
{
    [SerializeField] float inspectTime = 2;
    float inspectCooldown;
    bool atShore;
    
    protected override void Start() {
        base.Start();
        MoveTo(map.shore.position);
        inspectCooldown = inspectTime;
    }

    protected override void Update() {
        base.Update();
        if (atShore && !moving) {
            inspectCooldown -= Time.deltaTime;
            if (inspectCooldown <= 0) {
                inspectCooldown = inspectTime;
                MoveTo(map.station.position);
                FindObjectOfType<EventController>().LogInspector();
            }
        }
    }

    protected override void OnReachDestination() {
        base.OnReachDestination();
        if (targetPos == map.shore.position) atShore = true;
        else Destroy(gameObject);
    }
}

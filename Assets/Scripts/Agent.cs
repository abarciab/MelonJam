using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected Map map;
    [SerializeField] float speed;
    protected Vector3 targetPos;
    protected bool moving;
    public bool InWater;

    protected virtual void Start() {
        map = GameManager.i.map;
        transform.position = map.station.position;
    }

    protected void MoveTo(Vector3 position) {
        targetPos = position;
        moving = true;
    }

    protected virtual void Update() {
        if (moving) {
            var dir = targetPos - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
            if (Vector2.Distance(transform.position, targetPos) < 0.1f) {
                moving = false;
                OnReachDestination();
            }
        }
    }

    protected virtual void OnReachDestination() {}
}

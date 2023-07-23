using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform shore, station, waterline, watterBottom, waterLeft, waterRight, waterCenter;
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();

    public List<Vector3> GetPatrolPositions() {
        var list = new List<Vector3>();
        foreach (var p in patrolPoints) list.Add(p.position);
        return list;
    }
}

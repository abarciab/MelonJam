using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plankton : Creature
{
    [SerializeField] float speed;

    [SerializeField] Vector2 rotSpeedRange = new Vector2(-30, 30), rotTimeRange = new Vector2(0, 4);
    float rotTime, rotSpeed, delataRot;

    private void Update() {
        delataRot = Mathf.Lerp(delataRot, rotSpeed, 0.05f); 

        transform.eulerAngles += new Vector3(0, 0, delataRot * Time.deltaTime);
        rotTime -= Time.deltaTime;

        if (rotTime <= 0) {
            rotSpeed = Random.Range(rotSpeedRange.x, rotSpeedRange.y);
            rotSpeed *= rotSpeed;
            rotTime = Random.Range(rotTimeRange.x, rotTimeRange.y);
        }

        transform.position += transform.up * Time.deltaTime * speed;
    }
}

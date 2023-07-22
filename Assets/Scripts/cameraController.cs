using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class cameraController : MonoBehaviour
{
    [SerializeField] float upYPos;
    [SerializeField] float moveSmoothness = 0.05f;
    float downYPos;

    [Header("edit mode")]
    [SerializeField] bool goUp;
    [SerializeField] bool goDown;

    private void Update() {
        if (Application.isPlaying) return;

        if (goUp) {
            goUp = false;
            if (transform.position.y != upYPos) downYPos = transform.position.y;
            transform.position = new Vector3(transform.position.x, upYPos, transform.position.z);
        }

        if (goDown) {
            goDown = false;
            transform.position = new Vector3(transform.position.x, downYPos, transform.position.z);
        }
    }

    public void GoDown() {
        StartCoroutine(PanDown());
    }

    private void Start() {
        if (!Application.isPlaying) return;
        GameManager.i.OnDayEnd.AddListener(GoUp);
        downYPos = transform.position.y;
    }

    public void GoUp() {
        StopAllCoroutines();
        StartCoroutine(PanUp());
    }

    IEnumerator PanUp() {
        while (Mathf.Abs(transform.position.y - upYPos) > 0.01f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, upYPos, transform.position.z), moveSmoothness);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, upYPos, transform.position.z);
    }

    IEnumerator PanDown() {
        while (Mathf.Abs(transform.position.y - downYPos) > 0.01f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, downYPos, transform.position.z), moveSmoothness);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, downYPos, transform.position.z);
    }
}

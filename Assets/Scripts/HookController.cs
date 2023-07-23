using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [SerializeField] Transform hookEnd, aimCrosshair;
    LineRenderer line;
    [SerializeField] float hookSpeed;
    bool aiming;

    [SerializeField] Sound fireSound, travelSound, resetSound;

    private void Start() {
        fireSound = Instantiate(fireSound);
        travelSound = Instantiate(travelSound);
        resetSound = Instantiate(resetSound);

        line = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (!hookEnd) return;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, hookEnd.position);

        if (Input.GetMouseButtonDown(1)) aiming = true;

        GameManager.i.aimingHook = aiming;
        aimCrosshair.gameObject.SetActive(aiming);
        aimCrosshair.position = GetMousePos();

        if (Input.GetMouseButtonUp(1) && aiming) {
            StopAllCoroutines();
            StartCoroutine(FireHook());
        }
    }

    Vector3 GetMousePos() {
        var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.x);
        var pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        return pos;
    }

    IEnumerator FireHook() {
        fireSound.Play();
        travelSound.Play();
        Resource selectedResource = GameManager.i.HoveredResource;
        aiming = false;
        var pos = GetMousePos();

        hookEnd.up = (pos - transform.position).normalized;
        while (Vector2.Distance(hookEnd.position, pos) > hookSpeed * Time.deltaTime * 1.5f) {
            var dir = (pos - hookEnd.position).normalized;
            hookEnd.position += dir * hookSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (selectedResource != null) selectedResource.Harvest();

        while (Vector2.Distance(hookEnd.position, transform.position) > hookSpeed * Time.deltaTime * 1.5f) {
            var dir = (transform.position - hookEnd.position).normalized;
            hookEnd.position += dir * (hookSpeed/2) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        hookEnd.position = transform.position;
        resetSound.Play();
    }
}

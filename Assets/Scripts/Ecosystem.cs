using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem : MonoBehaviour
{
    public enum CreatureType { Plankton}

    //controls the spawning, pause/play, and virus interaction in the lake

    [SerializeField] int planktonCount;
    [SerializeField] GameObject planktonPrefab;
    [SerializeField] Transform planktonParent;
    [SerializeField] Vector2 bounds;

    List<Creature> creatures = new List<Creature>();

    Dictionary<KeyValuePair<Creature.Status, CreatureType>, int> creatureStatusDict = new Dictionary<KeyValuePair<Creature.Status, CreatureType>, int>();

    public int CheckStatus(Creature.Status statusCheck, CreatureType creatureCheck) {
        var key = new KeyValuePair<Creature.Status, CreatureType>(statusCheck, creatureCheck);
        if (creatureStatusDict.ContainsKey(key)) return creatureStatusDict[key];
        return 0;
    }

    public void ChangeStatusData(CreatureType creature, Creature.Status newStatus, Creature.Status oldStatus) {
        AddStatusData(creature, newStatus);
        RemoveStatusData(creature, oldStatus);
    }

    public void AddStatusData(CreatureType creature, Creature.Status status) {
        var key = new KeyValuePair<Creature.Status, CreatureType>(status, creature);
        if (creatureStatusDict.ContainsKey(key)) creatureStatusDict[key] += 1;
        else creatureStatusDict.Add(key, 1);
    }

    public void RemoveStatusData(CreatureType creature, Creature.Status status) {
        var key = new KeyValuePair<Creature.Status, CreatureType>(status, creature);
        creatureStatusDict[key] -= 1;
    }

    private void Start() {
        GameManager.i.OnDayEnd.AddListener(ResumeMovement);
        GameManager.i.OnGoDown.AddListener(PauseMovement);
        CheckCreatureLevels();
        PauseMovement();
    }

    void PauseMovement() {
        foreach (Transform child in planktonParent) PauseCreature(child);
    }

    void PauseCreature(Transform child) {
        child.GetComponent<Creature>().enabled = false;
    }

    void ResumeMovement() {
        foreach (Transform child in planktonParent) ResumeCreature(child);
        CheckCreatureLevels();
    }

    void ResumeCreature(Transform child) {
        child.GetComponent<Creature>().enabled = true;
    }

    void CheckCreatureLevels() {
        if (planktonParent.childCount != planktonCount) FixPopulation(planktonCount, planktonParent, planktonPrefab, CreatureType.Plankton);
    }

    void FixPopulation(int goalCount, Transform parent, GameObject prefab, CreatureType species) {
        while (parent.childCount < goalCount) {
            SpawnCreature(parent, prefab, species);
        }
        while (parent.childCount > goalCount) {
            KillCreature(parent);
        }
    }

    void KillCreature(Transform parent) {
        if (parent.childCount == 0) return;
        Destroy(parent.GetChild(Random.Range(0, parent.childCount)).gameObject);
    }

    void SpawnCreature(Transform parent, GameObject prefab, CreatureType species) {
        var newCreature = Instantiate(prefab, parent);
        newCreature.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        newCreature.transform.position += new Vector3(Random.Range(-bounds.x / 2, bounds.x / 2), Random.Range(-bounds.y / 2, bounds.y / 2));
        newCreature.GetComponent<Creature>().Init(this, species);
    }

    private void Update() {
        foreach (Transform plankton in planktonParent) {
            CheckBounds(plankton);
        }
    }

    void CheckBounds(Transform creature) {
        if (creature.position.x > transform.position.x + bounds.x/2) creature.position -= new Vector3(bounds.x, 0);
        if (creature.position.x < transform.position.x - bounds.x/2) creature.position += new Vector3(bounds.x, 0);
        if (creature.position.y > transform.position.y + bounds.y/2) creature.position -= new Vector3(0, bounds.y);
        if (creature.position.y < transform.position.y - bounds.y/2) creature.position += new Vector3(0, bounds.y);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}

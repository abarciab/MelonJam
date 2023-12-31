using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class Ecosystem : MonoBehaviour
{
    public enum CreatureType { Plankton, medFish, bigFish, shark, seaMonster}
     
    //controls the spawning, pause/play, and virus interaction in the lake

    [Space()]
    [SerializeField] int planktonCount;
    [SerializeField] GameObject planktonPrefab;
    [SerializeField] SortingGroup planktonSGroup;
    [SerializeField] Transform planktonParent;

    [Space()]
    [SerializeField] int medFishCount;
    [SerializeField] GameObject medFishPrefab;
    [SerializeField] SortingGroup medFishSGroup;
    [SerializeField] Transform medFishParent;

    [Space()]
    [SerializeField] int bigFishCount;
    [SerializeField] GameObject bigFishPrefab;
    [SerializeField] SortingGroup bigFishSGroup;
    [SerializeField] Transform bigFishParent;

    [Space()]
    [SerializeField] int SharkCount;
    [SerializeField] GameObject SharkPrefab;
    [SerializeField] SortingGroup SharkSGroup;
    [SerializeField] Transform SharkParent;

    [Space()]
    [SerializeField] Vector2 bounds;
    public Vector2 offset;

    [Header("Confrontation properties")]
    [SerializeField] float visibility;
    [SerializeField] float attackStrength;

    List<Creature> creatures = new List<Creature>();

    Dictionary<KeyValuePair<Creature.Status, CreatureType>, int> creatureStatusDict = new Dictionary<KeyValuePair<Creature.Status, CreatureType>, int>();

    public bool TooHigh(Vector2 pos) {
        return pos.y > ( transform.position.y + ((bounds.y / 2) * 0.8f));
    }

    public int CheckOnlyStatus(Creature.Status status, bool countPlankton = false) {
        int count = countPlankton ? CheckStatus(status, CreatureType.Plankton) : 0;
        count += CheckStatus(status, CreatureType.medFish);
        count += CheckStatus(status, CreatureType.shark);
        count += CheckStatus(status, CreatureType.seaMonster);
        return count;
    }

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
        HideShowCreatures(false);
    }

    void HideShowCreatures( bool active) {
        planktonSGroup.sortingLayerName = active ? "Default" : "Hidden";
        medFishSGroup.sortingLayerName = active ? "Default" : "Hidden";
    }

    void PauseMovement() {
        foreach (Transform child in planktonParent) PauseCreature(child);
        foreach (Transform child in medFishParent) PauseCreature(child);
        foreach (Transform child in bigFishParent) PauseCreature(child);
        foreach (Transform child in SharkParent) PauseCreature(child);
        HideShowCreatures(false);
    }

    void PauseCreature(Transform child) {
        child.GetComponent<Creature>().enabled = false;
    }

    void ResumeMovement() {
        foreach (Transform child in planktonParent) ResumeCreature(child);
        foreach (Transform child in medFishParent) ResumeCreature(child);
        foreach (Transform child in bigFishParent) ResumeCreature(child);
        foreach (Transform child in SharkParent) ResumeCreature(child);
        CheckCreatureLevels();
        HideShowCreatures(true);
    }    

    void ResumeCreature(Transform child) {
        child.GetComponent<Creature>().enabled = true;
    }

    void CheckCreatureLevels() {
        if (planktonParent.childCount != planktonCount) FixPopulation(planktonCount, planktonParent, planktonPrefab, CreatureType.Plankton);
        if (medFishParent.childCount != medFishCount) FixPopulation(medFishCount, medFishParent, medFishPrefab, CreatureType.medFish);
        if (bigFishParent.childCount != bigFishCount) FixPopulation(bigFishCount, bigFishParent, bigFishPrefab, CreatureType.bigFish);
        if (SharkParent.childCount != SharkCount) FixPopulation(SharkCount, SharkParent, SharkPrefab, CreatureType.shark);
    }

    void FixPopulation(int goalCount, Transform parent, GameObject prefab, CreatureType species) {
        while (parent.childCount < goalCount) {
            SpawnCreature(parent, prefab, species);
        }
        int childrenLeft = parent.childCount;
        while (childrenLeft > goalCount) {
            childrenLeft -= 1;
            KillCreature(parent);
        }
    }

    void KillCreature(Transform parent) {
        if (parent.childCount == 0) return;
        Destroy(parent.GetChild(Random.Range(0, parent.childCount)).gameObject);
    }

    void SpawnCreature(Transform parent, GameObject prefab, CreatureType species) {
        float edgeRespect = 0.8f;
        var newCreature = Instantiate(prefab, parent);
        newCreature.transform.position += (Vector3) offset;
        newCreature.transform.position += new Vector3(Random.Range((-bounds.x / 2) * edgeRespect, (bounds.x / 2) * edgeRespect), Random.Range((-bounds.y / 2) * edgeRespect, (bounds.y / 2) * edgeRespect));
        newCreature.GetComponent<Creature>().Init(this, species);
    }

    private void Update() {
        foreach (Transform plankton in planktonParent) {
            CheckBounds(plankton);
        }
        planktonParent.transform.localPosition = offset;

        foreach (Transform medFish in medFishParent) {
            CheckBounds(medFish);
        }
        medFishParent.transform.localPosition = offset;
    }

    void CheckBounds(Transform creature) {
        float x = transform.position.x + offset.x;
        float y = transform.position.y + offset.y;
        var pos = creature.position;
        bool top = false;
        if (pos.x > x + bounds.x / 2) {
            pos = new Vector3(-bounds.x, 0);
            top = true;
        }
        else if (pos.x < x - bounds.x / 2) pos = new Vector3(bounds.x, 0);
        else if (pos.y > y + bounds.y / 2) pos = new Vector3(0, -bounds.y);
        else if (pos.y < y - bounds.y / 2) pos = new Vector3(0, bounds.y);
        else pos = Vector2.zero;

        creature.position += pos;
        if (top) creature.GetComponent<Creature>().OnWrapAround(true);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, bounds);
    }
}

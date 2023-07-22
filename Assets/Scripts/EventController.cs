using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [System.Serializable]
    public class Event {
        [HideInInspector] public string name;
        public enum TriggerType {healthyCount, infectedCount}

        public TriggerType triggerType;
        public Ecosystem.CreatureType creatureType;
        public float amount;
        public string headline;
        [HideInInspector] public bool triggered;

        public Creature.Status StatusTypeCheck() {
            switch (triggerType) {
                case TriggerType.healthyCount:
                    return Creature.Status.healthy;
                    
                case TriggerType.infectedCount:
                    return Creature.Status.infected;
            }
            return Creature.Status.healthy;
        }
    }

    [SerializeField] List<Event> events = new List<Event>();
    [SerializeField] float eventSpaceTime = 1;

    GameManager gMan;
    Ecosystem eco;

    private void Start() {
        gMan = GameManager.i;
        gMan.OnGoDown.AddListener(CheckEvents);
        eco = gMan.eco;
    }

    void CheckEvents() {
        List<Event> triggeredEvents = new List<Event>();
        foreach (var e in events) {
            if (!e.triggered && eco.CheckStatus(e.StatusTypeCheck(), e.creatureType) > e.amount) triggeredEvents.Add(e);
        }
        StartCoroutine(TriggerEvents(triggeredEvents));
    }

    IEnumerator TriggerEvents(List<Event> events) {
        for (int i = 0; i < events.Count; i++) {
            TriggerEvent(events[i]);
            events[i].triggered = true;
            yield return new WaitForSeconds(eventSpaceTime);
        }
    }

    void TriggerEvent(Event e) {
        gMan.headline.DisplayHeadline(e.headline);
    }
}

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

    [System.Serializable]
    public class InspectorPersonality {
        [HideInInspector] public string name;
        public CharacterType type;
        public List<SpeciesTrigger> speciesTriggers = new List<SpeciesTrigger>();
        public string defaultHeadline;
        
        [System.Serializable]
        public class SpeciesTrigger
        {
            public Ecosystem.CreatureType species;
            public List<StatusTrigger> statusTrigger = new List<StatusTrigger>();

            [System.Serializable]
            public class StatusTrigger
            {
                public Creature.Status status;
                public int amount;
                public float alarm;
                public string headline;
            }
        }
    }


    //[SerializeField]

    [SerializeField] List<Event> events = new List<Event>();
    [SerializeField] float eventSpaceTime = 1;

    public enum CharacterType { fisherman, scientist};
    [Header("Daily inspector visits")]
    [SerializeField] List<CharacterType> scheduledInspectors = new List<CharacterType>();
    [SerializeField] List<InspectorPersonality> inspectors = new List<InspectorPersonality>();
    CharacterType currentInspector;
    bool dontSendInspector;

    ConfrontationDisplay InspectorDisplay;
    GameManager gMan;
    Ecosystem eco;

    private void OnValidate() {
        foreach (var i in inspectors) i.name = i.type.ToString();
    }

    private void Start() {
        gMan = GameManager.i;
        gMan.OnGoDown.AddListener(CheckEvents);
        eco = gMan.eco;
        InspectorDisplay = FindObjectOfType<ConfrontationDisplay>();
        gMan.OnDayEnd.AddListener(SendInspector);
    }

    void SendInspector() {
        if (dontSendInspector) {
            dontSendInspector = false;
            return;
        }

        InspectorDisplay.SendInspector(scheduledInspectors[0]);

        var inspector = scheduledInspectors[0];
        scheduledInspectors.Add(scheduledInspectors[0]);
        scheduledInspectors.RemoveAt(0);
        currentInspector = inspector;   
    }

    public void LogInspector() {
        CheckAlarms(currentInspector);
    }

    public void DontSendInspectorTomorrow() {
        dontSendInspector = true;
    }

    void CheckAlarms(CharacterType type) {
        foreach (var i in inspectors) {
            if (i.type == type) CheckAlarm(i);
        }
    }

    void CheckAlarm(InspectorPersonality inspector) {
        float suspicionBefore = gMan.suspicion;
        foreach (var t in inspector.speciesTriggers) CheckSpeciesTriggers(t);
        if (gMan.suspicion == suspicionBefore) gMan.headline.DisplayHeadline(inspector.defaultHeadline);
    }

    void CheckSpeciesTriggers(InspectorPersonality.SpeciesTrigger speciesTrigger) {
        foreach (var status in speciesTrigger.statusTrigger) {
            float amount = eco.CheckStatus(status.status, speciesTrigger.species);
            if (amount > status.amount) {
                gMan.IncreaseSuspicion(status.alarm);
                gMan.headline.DisplayHeadline(status.headline);
            }
            //print("there are " + amount + " " + status.status + " " + speciesTrigger.species);
        }
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

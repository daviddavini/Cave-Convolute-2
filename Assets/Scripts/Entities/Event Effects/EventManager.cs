using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType {None, OnHurt, OnHeal, OnMove, OnDie, ScorePoints,
  GameOver, OnIdle, OnCarried, OnThrown, OnLand, OnCarry, OnThrow, OnAttack, OnStore,
  OnSpawn, OnGateOpen};

public class EventManager : MonoBehaviour
{
    private Dictionary<GameObject, Dictionary<EventType, Action>> eventDictionaries
      = new Dictionary<GameObject, Dictionary<EventType, Action>>();

    public void StartListening(EventType eventType, Action listener, GameObject triggerer = null)
    {
        // For this to work, eventmanager needs to be on an object by itself
        if(triggerer == null) {triggerer = gameObject;}
        Dictionary<EventType, Action> eventDictionary;
        if (eventDictionaries.TryGetValue(triggerer, out eventDictionary))
        {
            Action thisEvent;
            if (eventDictionary.TryGetValue(eventType, out thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;
                //Update the Dictionary
                eventDictionary[eventType] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                eventDictionary.Add(eventType, thisEvent);
            }
        }
        else
        {
            Action thisEvent = delegate {};
            thisEvent += listener;
            eventDictionaries.Add(triggerer, new Dictionary<EventType, Action>(){{eventType, thisEvent}});
        }
    }

    public void StopListening(EventType eventType, Action listener, GameObject triggerer = null)
    {
        if(triggerer == null) {triggerer = gameObject;}
        Dictionary<EventType, Action> eventDictionary;
        if (eventDictionaries.TryGetValue(triggerer, out eventDictionary))
        {
            Action thisEvent;
            if (eventDictionary.TryGetValue(eventType, out thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;
                //Update the Dictionary
                eventDictionary[eventType] = thisEvent;
            }
        }
    }

    public void TriggerEvent(EventType eventType, GameObject triggerer = null)
    {
        if(triggerer == null){
            triggerer = gameObject;
            Debug.Log(eventType);
        }
        Dictionary<EventType, Action> eventDictionary = null;
        if (eventDictionaries.TryGetValue(triggerer, out eventDictionary))
        {
            Action thisEvent = null;
            if (eventDictionary.TryGetValue(eventType, out thisEvent))
            {
                thisEvent.Invoke();
                // OR USE instance.eventDictionary[eventName]();
            }
        }
    }
}

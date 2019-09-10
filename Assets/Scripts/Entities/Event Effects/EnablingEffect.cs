using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablingEffect : MonoBehaviour
{
    public List<MonoBehaviour> scriptsToEnable;

    public EventType enableEventType;
    public EventType disableEventType;

    private EventManager eventManager;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }
    void OnEnable()
    {
        eventManager.StartListening(enableEventType, EnableScript, gameObject);
        eventManager.StartListening(disableEventType, DisableScript, gameObject);
    }
    void OnDisable()
    {
        eventManager.StopListening(enableEventType, EnableScript, gameObject);
        eventManager.StopListening(disableEventType, DisableScript, gameObject);
    }
    void EnableScript()
    {
        foreach(MonoBehaviour scriptToEnable in scriptsToEnable)
        {
            scriptToEnable.enabled = true;
        }
    }
    void DisableScript()
    {
        foreach(MonoBehaviour scriptToEnable in scriptsToEnable)
        {
            scriptToEnable.enabled = false;
        }
    }
}

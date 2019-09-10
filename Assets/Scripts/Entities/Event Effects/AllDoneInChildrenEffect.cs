using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDoneInChildrenEffect : MonoBehaviour
{
    int todos;
    private EventManager eventManager;
    public EventType eventType;

    void Awake()
    {

        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
        foreach(Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
        {
            if(rb == GetComponent<Rigidbody2D>()) continue;
            todos++;
            eventManager.StartListening(eventType, DecrementTodos, rb.gameObject);
        }
    }
    void DecrementTodos()
    {
        todos--;
        if(todos == 0)
        {
            eventManager.TriggerEvent(eventType, gameObject);
        }
    }
}

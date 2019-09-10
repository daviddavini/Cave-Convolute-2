using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePoints : MonoBehaviour
{
    EventManager eventManager;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            eventManager.TriggerEvent(EventType.ScorePoints);
            Destroy(gameObject);
          //Debug.Log("List size: "+collidingObjects.Count);
        }
    }
}

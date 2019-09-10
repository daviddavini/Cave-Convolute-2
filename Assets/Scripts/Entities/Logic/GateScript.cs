using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    private EventManager eventManager;

    public string targetTag;

    private void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D otherRigidbody = collision.GetComponentInParent<Rigidbody2D>();
        if (otherRigidbody == null) return;
        GameObject otherGameObject = otherRigidbody.gameObject;
        if (otherGameObject.CompareTag(targetTag))
        {
            eventManager.TriggerEvent(EventType.OnGateOpen, gameObject);
        }
    }
}

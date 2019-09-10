using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [System.Serializable]
// public class HealthChangeEvent : UnityEvent<float, float>{}

public class Health : MonoBehaviour
{
    private EventManager eventManager;

    private bool isDead = false;
    public float maxHealth = 1;
    public float health = 0;
    public event Action<float, float> NewHealthEvent = delegate {};
    public bool isInvincible = false;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
        health = health<=0 ? maxHealth : health;
        NewHealthEvent(health, 0);
    }

    void Die()
    {
        isDead = true;
        if (eventManager) {
            eventManager.TriggerEvent(EventType.OnDie, gameObject);
        }

    }

    public bool DoHealthChange(float healthChange)
    {
        float oldHealth = health;
        if (isDead || healthChange == 0) {return false;}
        if (eventManager) {
            eventManager.TriggerEvent((healthChange > 0) ? EventType.OnHeal : EventType.OnHurt, gameObject);
        }
        //float oldHealth = health;
        health += healthChange;
        health = Tools.BoundedFloat(health, 0, maxHealth);
        if (health == 0 && !isInvincible)
        {
            Die();
        }
        NewHealthEvent(health, healthChange);
        return !(oldHealth == health);
    }
}

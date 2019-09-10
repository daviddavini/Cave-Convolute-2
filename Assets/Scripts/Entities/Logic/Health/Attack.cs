using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//eventually replace GO list with health list for simplicity

public class Attack : MonoBehaviour
{
    public float damage;
    public float maxDamageBonus = 0;
    public string targetTag = "Player";
    private List<GameObject> collidingObjects = new List<GameObject>();
    public float attackTimeDelay = 1;
    private Dictionary<Health, float> nextAttackTimes = new Dictionary<Health, float>();
    private EventManager eventManager;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ///!!! Important! This moves up to the game object that you want
        Rigidbody2D otherRigidbody = other.GetComponentInParent<Rigidbody2D>();
        if(otherRigidbody == null) return;
        GameObject otherGameObject = otherRigidbody.gameObject;
        if (otherGameObject.tag == targetTag)
        {
          collidingObjects.Add(otherGameObject);
          //Debug.Log("List size: "+collidingObjects.Count);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D otherRigidbody = other.GetComponentInParent<Rigidbody2D>();
        if(otherRigidbody == null) return;
        GameObject otherGameObject = otherRigidbody.gameObject;
        collidingObjects.Remove(otherGameObject);
    }

    void RemoveExpiredHealthProtections()
    {
        List<Health> expiredHealths = new List<Health>();
        foreach(var entry in nextAttackTimes)
        {
            if (Time.time > entry.Value)
            {
                expiredHealths.Add(entry.Key);
            }
        }
        foreach(Health health in expiredHealths)
        {
            nextAttackTimes.Remove(health);
        }
    }

    void RemoveDestroyedObjects()
    {
        collidingObjects.RemoveAll((c) => c==null);
    }

    void AttackCollidingObjects()
    {
        foreach (GameObject collidingObject in collidingObjects)
        {
            Health health = collidingObject.GetComponentInParent<Health>();
            if(!nextAttackTimes.ContainsKey(health))
            {
                nextAttackTimes.Add(health, Time.time + attackTimeDelay);
                if(health.DoHealthChange(-(damage + Random.Range(0, maxDamageBonus))))
                    eventManager.TriggerEvent(EventType.OnAttack, gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RemoveDestroyedObjects();
        RemoveExpiredHealthProtections();
        AttackCollidingObjects();
    }
}

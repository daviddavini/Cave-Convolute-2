using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum CarryState {None, Carried, Thrown};
//Coupling Carrying and Throwning? Seems okay
public class Carriable : MonoBehaviour
{
    public Stats statsChangeOnStored;

    //private CarryState carryState = CarryState.None;
    private bool isInteractable = true;
    private bool isFlying = false;
    //private bool isThrown = false;
    private EventManager eventManager;

    private SpriteRenderer spriteRenderer;
    new private Rigidbody2D rigidbody;

    private float nextLandTime;
    private float oldGravityScale;
    private float oldLinearDrag;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public bool BeCarriedBy(GameObject carrierObject)
    {
        if (!isInteractable) {return false;}
        BecomeUninteractable();
        eventManager.TriggerEvent(EventType.OnCarried, gameObject);
        return true;
    }

    public bool BeUncarried()
    {
        BecomeInteractable();
        return true;
    }

    public bool BeThrownTo(float throwHeight, Vector3 targetPosition, float gravityZ)
    {
        float aZ = gravityZ;
        Vector3 delta = targetPosition - transform.position;

        //Check if throw height can handle this distance
        float maxDistance = 2*throwHeight;
        if(delta.magnitude > maxDistance)
        {
            delta *= maxDistance/delta.magnitude;
        }
        float throwSpeed = 2*Mathf.Sqrt(-aZ*throwHeight);

        //Make projectile path by potential throw velocity
        float[] tSquareds = quadraticFormula(
          0.25f*aZ*aZ,
          -throwSpeed*throwSpeed,
          delta.magnitude*delta.magnitude);
        if (tSquareds == null){return false;}
        float t = Mathf.Sqrt(Mathf.Min(tSquareds));

        Vector3 v = delta / t;
        v.y += -0.5f*Physics2D.gravity.y*t;//(delta.y - 0.5f*a*t*t) / t;

        StartFlight(v, t);

        return true;
    }

    private void BecomeUninteractable()
    {
        isInteractable = false;
        foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            if(!collider.isTrigger){collider.enabled = false;}
        }
        spriteRenderer.sortingLayerName = "Air";
        //Debug.Log(spriteRenderer.sortingLayerName);
    }
    private void BecomeInteractable()
    {
        isInteractable = true;
        foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            if(!collider.isTrigger){collider.enabled = true;}
        }
        spriteRenderer.sortingLayerName = "Default";
        //Debug.Log(spriteRenderer.sortingLayerName);
    }

    private float[] quadraticFormula(float a, float b, float c)
    {
        float disc = b*b-4*a*c;
        if(disc < 0){Debug.Log("Impossible Throw: "+disc); return null;}
        return new float[] {(-b+Mathf.Sqrt(disc))/(2*a), (-b-Mathf.Sqrt(disc))/(2*a)};
    }

    void StartFlight(Vector3 velocity, float time)
    {
        isFlying = true;
        rigidbody.velocity = velocity;
        nextLandTime = Time.time + time;

        //Potentially a problem, may need to be generalized/fixed
        oldGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = 1;
        oldLinearDrag = rigidbody.drag;
        rigidbody.drag = 0;

        eventManager.TriggerEvent(EventType.OnThrown, gameObject);
    }

    void EndFlight()
    {
        isFlying = false;
        rigidbody.velocity = Vector3.zero;

        rigidbody.drag = oldLinearDrag;
        rigidbody.gravityScale = oldGravityScale;

        BecomeInteractable();

        eventManager.TriggerEvent(EventType.OnLand, gameObject);
    }

    void Update()
    {
        if(isFlying && Time.time > nextLandTime)
        {
            EndFlight();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierController : MonoBehaviour
{
    private Carrier carrier;
    public bool handleCarries = true;
    public bool handleThrows = true;
    //replace with more flexible metric (ex. on screen search)
    public float carrySearchDistance = 3;

    protected virtual void Awake()
    {
        carrier = GetComponent<Carrier>();
    }

    protected virtual void Update(){}

    protected void HandleThrowAtPoint(Vector3 position)
    {
        if (!handleThrows) return;

        carrier.DoThrow((Vector2) position);
    }

    protected void HandleCarryAtPoint(Vector3 position)
    {
        if(!handleCarries) return;

        RaycastHit2D[] hitInformations = Physics2D.RaycastAll(position, Camera.main.transform.forward);
        foreach (RaycastHit2D hitInformation in hitInformations)
        {
            if (hitInformation.collider != null)
            {
                GameObject clickedObject = hitInformation.transform.gameObject;
                carrier.DoCarry(clickedObject);
            }
        }
    }

    protected void HandleCarryBySearch()
    {
        if(!handleCarries) return;

        GameObject closest = Tools.FindClosest(transform.position,
          (go) => go.GetComponentInParent<Carriable>(), carrySearchDistance);
        if(closest) {
            Debug.Log(closest.activeSelf + " " + closest.activeInHierarchy);
            GameObject closestCarriableGameObject = closest.GetComponentInParent<Carriable>().gameObject;
            carrier.DoCarry(closestCarriableGameObject);
        }
    }
}

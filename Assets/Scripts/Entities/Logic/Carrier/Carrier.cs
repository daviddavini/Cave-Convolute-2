using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{

    public Transform onHand;
    private Carriable carriable;
    public bool isCarrying = false;
    private GameObject _CarriedObject;
    private GameObject CarriedObject
    {
      get
      {
          return _CarriedObject;
      }
      set
      {
          if(value == null || value.GetComponent<Carriable>() == null)
          {
              _CarriedObject = null;
              carriable = null;
              isCarrying = false;
              Destroy(GetComponent<FixedJoint2D>());
          }
          else
          {
              _CarriedObject = value;
              carriable = value.GetComponent<Carriable>();
              isCarrying = true;
              _CarriedObject.transform.position = onHand.position;
              _CarriedObject.transform.parent = transform.parent;
              FixedJoint2D fixedJoint = gameObject.AddComponent<FixedJoint2D>();
              fixedJoint.connectedBody = _CarriedObject.GetComponent<Rigidbody2D>();
              //Debug.Log(fixedJoint, _CarriedObject);
          }
      }
    }
    private EventManager eventManager;
    public float carryPickupDistance = 1;
    public float throwStraightness = 1;

    //public event Action<GameObject>

    public float throwHeight = 3f;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    public GameObject PauseCarry()
    {
        if (!isCarrying) return null;

        //carriable.BeUncarried();
        CarriedObject.SetActive(false);
        GameObject previouslyCarriedObject = CarriedObject;
        //Debug.Log(CarriedObject.activeSelf + "" + CarriedObject.activeInHierarchy);
        CarriedObject = null;
        return previouslyCarriedObject;
    }

    public void ContinueCarry(GameObject objectToCarry)
    {
        if (isCarrying) return;
        objectToCarry.SetActive(true);
        CarriedObject = objectToCarry;
    }

    public void DoCarry(GameObject pickupObject)
    {
        //Debug.Log("trying carry");
        if (isCarrying) return;

        //WARNING This isn't working - fix it
        if (!pickupObject.activeSelf || !pickupObject.activeInHierarchy) return;
        // if ((pickupObject.transform.position - transform.position).magnitude > carryPickupDistance)
        //     return;

        CarriedObject = pickupObject;

        if (isCarrying && carriable.BeCarriedBy(gameObject))
        {
            Debug.Log("Pickup" + eventManager);
            eventManager.TriggerEvent(EventType.OnCarry, gameObject);
        }
    }

    //eventually split this away from coupling with carry
    public void DoThrow(Vector3 targetPosition)
    {
        float gravityZ = -throwStraightness;
        if(isCarrying && carriable.BeThrownTo(throwHeight, targetPosition, gravityZ))
        {
            eventManager.TriggerEvent(EventType.OnThrow, gameObject);
            CarriedObject = null;
        }
    }
}

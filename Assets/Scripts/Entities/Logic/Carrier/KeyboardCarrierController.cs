using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rename and split
public class KeyboardCarrierController : CarrierController
{
    //private pickupDistance = 2;
    private Movement movement;
    public float targetDistance = 3;
    public KeyCode carryKey = KeyCode.N;
    public KeyCode storeKey = KeyCode.M;
    public KeyCode throwKey = KeyCode.Space;

    private Inventory inventory;
    //private bool pressedCarryKey = false;

    protected override void Awake()
    {
        movement = GetComponent<Movement>();
        base.Awake();

        inventory = GetComponent<Inventory>();
    }

    protected override void Update()
    {
        base.Update();
        //duplicate, split into carry control and throw control
        if (Input.GetKeyDown(carryKey))
        {
            Debug.Log("keyboard carry");
            HandleCarryBySearch();
        }
        if (Input.GetKeyDown(throwKey))
        {
            Vector2 position = transform.position + targetDistance * movement.direction;
            HandleThrowAtPoint(position);
        }
        if (Input.GetKeyDown(storeKey))
        {
            inventory.SmartToggle();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCarrierController : CarrierController
{
    public bool pointCarry = false;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //If click, initiate a pickup
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(!IsPointerOnUIControls())
                HandleThrowAtPoint(clickPositionInWorld);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(pointCarry)
            {
                Vector3 clickPositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HandleCarryAtPoint(clickPositionInWorld);
            } else {
                Debug.Log("trying");
                HandleCarryBySearch();
            }
        }
    }

    bool IsPointerOnUIControls()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        foreach(var hit in raycastResults)
        {
          if(hit.gameObject.CompareTag("Inventory")
            || hit.gameObject.CompareTag("Move Joystick")
            || hit.gameObject.CompareTag("Carry Button")
            || hit.gameObject.CompareTag("Store Button"))
              return true;
        }
        return false;
    }
}

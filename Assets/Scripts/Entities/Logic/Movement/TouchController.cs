using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MovementController
{
    private Carrier carrier;
    private float minimumDragSize = 10;
    private int primaryTouchFingerId = -1;
    private IDictionary<int, Vector2> touchStartPositions = new Dictionary<int, Vector2>();

    protected override void Awake()
    {
        base.Awake();
        carrier = GetComponent<Carrier>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            TouchPhase phase = touch.phase;
            if (phase == TouchPhase.Began)
            {
                //Keep track of the first touch
                if (touchStartPositions.Count == 0)
                {
                    primaryTouchFingerId = touch.fingerId;
                }
                //Add touch to dictionary (regardless)
                touchStartPositions.Add(touch.fingerId, touch.position);
            }
            else
            {
                //Interpret touch as "drag" (first touch only)
                if (primaryTouchFingerId == touch.fingerId)
                {
                    Vector3 displacement = GetDisplacement(touch);
                    if (displacement.magnitude >= minimumDragSize)
                    {
                        SetNextMove(displacement.normalized);
                    }
                }
                //Remove touch when finished
                if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
                {
                    //Interpret touches as "taps" or "drags"
                    Vector3 displacement = GetDisplacement(touch);
                    if (displacement.magnitude >= minimumDragSize)
                    {
                        SetNextMove(displacement.normalized);
                    }
                    else if (displacement.magnitude <= minimumDragSize)
                    {
                        Vector3 touchPositionInWorld = Camera.main.ScreenToWorldPoint(touchStartPositions[touch.fingerId]);
                        RaycastHit2D hitInformation = Physics2D.Raycast(touchPositionInWorld,
                          Camera.main.transform.forward);

                        carrier.DoThrow((Vector2)touchPositionInWorld);
                        if (hitInformation.collider != null)
                        {
                            GameObject touchedObject = hitInformation.transform.gameObject;
                            carrier.DoCarry(touchedObject);
                        }
                    }
                    //Free up the first touch variable
                    if (primaryTouchFingerId == touch.fingerId)
                    {
                        primaryTouchFingerId = -1;
                    }
                    //Remove touch from dictionary
                    touchStartPositions.Remove(touch.fingerId);
                }
            }
        }
    }

    Vector2 GetDisplacement(Touch touch)
    {
        Vector2 startPosition = touchStartPositions[touch.fingerId];
        return new Vector3(touch.position.x-startPosition.x,
          touch.position.y-startPosition.y, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float weight = 1;

    private Movement movement;
    private bool hasNextMove = false;
    private Vector3 nextMoveDirection;

    protected virtual void Awake()
    {
        movement = GetComponent<Movement>();
    }

    protected void SetNextMove(Vector3 moveDirection)
    {
        nextMoveDirection = moveDirection;
        hasNextMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hasNextMove)
        {
            movement.ScheduleMove(nextMoveDirection * weight);
            hasNextMove = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    new public Rigidbody2D rigidbody;
    private List<Vector3> scheduledMoves = new List<Vector3>();
    //private Vector3 nextMove;
    private EventManager eventManager;
    //public event Action<Vector3> MoveEvent = delegate {};

    protected virtual void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
        rigidbody = rigidbody == null ? GetComponent<Rigidbody2D>() : rigidbody;
        //MoveEvent += DoMove;
    }

    public void ScheduleMove(Vector3 moveDirection)
    {
        scheduledMoves.Add(moveDirection);
    }

    private void DoMove(Vector3 moveDirection)
    {
        rigidbody.AddForce(moveDirection*speed, ForceMode2D.Impulse);
        if (eventManager)
        {
            if (moveDirection != Vector3.zero)
            {
                direction = moveDirection;
                eventManager.TriggerEvent(EventType.OnMove, gameObject);
            } else
            {
                eventManager.TriggerEvent(EventType.OnIdle, gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        if(scheduledMoves.Count > 0)
        {
            Vector3 totalMoves = Vector3.zero;
            float totalMovesMagnitude = 0.01f;   //to prevent division by zero
            foreach(Vector3 move in scheduledMoves)
            {
                totalMoves += move;
                totalMovesMagnitude += move.magnitude;
            }
            DoMove(totalMoves/totalMovesMagnitude);
            scheduledMoves.Clear();
        }
        else
        {
            DoMove(Vector3.zero);
        }
    }
}

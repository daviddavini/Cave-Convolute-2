using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovementController : MovementController
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"),
          Input.GetAxisRaw("Vertical"), 0);

        SetNextMove(moveDirection);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMovementController : MovementController
{
    private Joystick joystick;

    protected override void Awake()
    {
        base.Awake();
        joystick = GameObject.FindWithTag("Move Joystick").GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(joystick.Horizontal,
          joystick.Vertical, 0);

        SetNextMove(moveDirection);
    }
}

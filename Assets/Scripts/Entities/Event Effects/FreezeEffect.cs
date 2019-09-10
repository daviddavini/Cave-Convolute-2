using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : EventEffect
{
    private Movement movement;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<Movement>();
    }

    protected override void DoEffect()
    {
        movement.enabled = false;
    }
    protected override void UndoEffect()
    {
        movement.enabled = true;
    }
}

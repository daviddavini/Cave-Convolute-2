using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeMovementController : MovementController
{
    public float redirectTimeDelay = 1;
    private Vector3 direction;

    void OnEnable()
    {
        StartCoroutine(RedirectTick());
    }

    IEnumerator RedirectTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(redirectTimeDelay);

            float angle = Random.Range(-3.14f, 3.14f);
            direction = Tools.UnitVector(angle);
        }
    }

    void Update()
    {
        SetNextMove(direction);
    }
}

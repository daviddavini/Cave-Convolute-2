using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : EventEffect
{
    //public GameObject gameObjectToDestroy = null;

    protected override void Awake()
    {
        base.Awake();
        //if(gameObjectToDestroy == null) {gameObjectToDestroy = gameObject;}
    }

    protected override void DoEffect()
    {
        Debug.Log("destroy: " + (eventGameObject));
        Destroy(eventGameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekMovementController : MovementController
{

    public float sightDistance = 3;
    private GameObject prey = null;
    public string preyTag = "Player";
    public GameObject preyObject;
    public float searchTimeDelay = 1;

    protected void OnEnable()
    {
        StartCoroutine(FindPreyTick(sightDistance));
    }

    // Update is called once per frame
    IEnumerator FindPreyTick(float radius)
    {
        while(true)
        {
            yield return new WaitForSeconds(searchTimeDelay);

            if (preyObject != null)
                prey = preyObject;
            else
                prey = Tools.FindClosest(transform.position, (go) => go.tag == preyTag, radius);
            //if(gameObject.name == "Bat(Clone)") Debug.Log(prey);
        }
    }

    void Update()
    {
        if (prey){SetNextMove((prey.transform.position - transform.position).normalized);}
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,0,0,0.3f);
        Gizmos.DrawSphere(transform.position, sightDistance);
    }
}

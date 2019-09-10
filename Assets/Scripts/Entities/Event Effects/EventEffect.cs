using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEffect : MonoBehaviour
{
    public EventType effectEvent;
    public bool isEventGlobal = false;
    public GameObject eventGameObject;
    public float timeDelay = 0;
    public float undoTimeDelay = Mathf.Infinity;
    protected EventManager eventManager;
    public bool overlapHappening = true;
    protected bool isHappening = false;

    protected virtual void Awake()
    {
        if (eventGameObject==null)
        {
            eventGameObject = gameObject;
        }
        if (isEventGlobal)
        {
            eventGameObject = null;
        }
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    protected virtual void OnEnable()
    {
        eventManager.StartListening(effectEvent, InitiateEffect, eventGameObject);
    }

    protected virtual void OnDisable()
    {
        if (eventManager != null) eventManager.StopListening(effectEvent, InitiateEffect, eventGameObject);
    }

    void InitiateEffect()
    {
        if(!isHappening || overlapHappening)
        {
            isHappening = true;
            StartCoroutine(DoAndUndoEffect());
        }
    }

    IEnumerator DoAndUndoEffect()
    {
        if(timeDelay > 0)
        {
            yield return new WaitForSeconds(timeDelay);
        }
        DoEffect();
        if(undoTimeDelay != Mathf.Infinity)
        {
          yield return new WaitForSeconds(undoTimeDelay);
          UndoEffect();
        }
        isHappening = false;
    }

    protected virtual void DoEffect(){}
    protected virtual void UndoEffect(){}
}

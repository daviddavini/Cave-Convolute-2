using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//eventually merge with other effects
public class OrientEffect : MonoBehaviour
{
    //public Vector3 defaultDirection = Vector3.up;
    public bool isSmoothRotation = true;
    public float rotationSpeed = 10;

    private GameObject eventGameObject;
    //private Quaternion defaultRotation;
    private bool isActive = false;
    private Vector3 targetDirection;
    private EventManager eventManager;

    private new Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
        //defaultRotation = transform.rotation;
        rigidbody = GetComponentInParent<Rigidbody2D>();
        eventGameObject = rigidbody.gameObject;
    }
    void OnEnable()
    {
        eventManager.StartListening(EventType.OnMove, PointTowardsVelocity, eventGameObject);
        //eventManager.StartListening(EventType.OnIdle, PointUp, eventGameObject);
    }
    void OnDisable()
    {
        if(eventManager) eventManager.StopListening(EventType.OnMove, PointTowardsVelocity);
        //eventManager.StopListening(EventType.OnIdle, PointUp);
    }

    void PointUp()
    {
        //Debug.Log("Pointing Up");
        isActive = true;
        targetDirection = transform.parent.up;
    }

    void PointTowardsVelocity()
    {
        //Debug.Log("Pointing Towards "+rigidbody.velocity.normalized);
        isActive = true;
        targetDirection = rigidbody.velocity.normalized;
    }

    void Update()
    {
        if (isActive)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation,
              Quaternion.FromToRotation (transform.parent.up, targetDirection),
              rotationSpeed * Time.deltaTime);
            if (transform.up == targetDirection)
            {
                isActive = false;
            }
        }
    }

    void DrawGizmos()
    {
        if(isActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position+transform.up);
            Gizmos.DrawLine(transform.position, transform.position+targetDirection);
        }
    }
}

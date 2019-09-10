using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject cameraTarget;
    public float speed = 2.0f;
    public float smoothTime = 1f;
    public bool isSmooth = true;
    private Vector3 offset;

    public Vector2 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position - cameraTarget.transform.position;
        offset = new Vector3(0,0,0);
        cameraTarget = cameraTarget != null ? cameraTarget : GameObject.FindWithTag("Camera Target");
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if(!isSmooth)
        {
            Vector2 newPosition = Vector2.Lerp(this.transform.position,
              cameraTarget.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }else
        {
            Vector2 newPosition = Vector2.SmoothDamp(transform.position,
              cameraTarget.transform.position, ref velocity, smoothTime);
            //Setting position = newposition causes camera to be black (too close)
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }
}

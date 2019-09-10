using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class FlickerLight : MonoBehaviour
{
    private new Light2D light;
    public float maxIntensityOffset;
    public float baseIntensity;
    public Vector2 timeBetweenFlickersRange;
    public Vector2 flickerDurationRange;
    private float nextTime = 0;
    private bool isActive = false;

    void Awake()
    {
        light = GetComponent<Light2D>();
        baseIntensity = light.intensity;
    }

    void Update()
    {
        if (Time.time > nextTime)
        {
            if(isActive)
            {
                light.intensity = baseIntensity;
                nextTime = Time.time + Random.Range(timeBetweenFlickersRange.x, timeBetweenFlickersRange.y);
                isActive = false;
            }
            else
            {
                light.intensity = baseIntensity == 0 ? 0 : baseIntensity + maxIntensityOffset*Random.Range(-1f, 1);
                nextTime = Time.time + Random.Range(flickerDurationRange.x, flickerDurationRange.y);
                isActive = true;
            }
        }
    }
}

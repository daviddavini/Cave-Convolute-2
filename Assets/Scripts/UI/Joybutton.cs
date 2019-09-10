using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joybutton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed;
    public event Action ButtonDownEvent = delegate {};

    void Awake()
    {
    }

    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        IsPressed = true;
        ButtonDownEvent();
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        IsPressed = false;
    }
}

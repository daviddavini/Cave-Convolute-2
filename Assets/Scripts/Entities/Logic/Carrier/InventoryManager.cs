using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    public Sprite defaultSlotSprite = null;
    public Color defaultSlotColor = new Color(0,0,0,0);

    void Awake()
    {
        inventory.InventoryUpdateEvent += RefreshImages;
        int slot = 0;
        foreach(Transform child in transform)
        {
            child.GetChild(0).GetComponent<Button>().onClick.AddListener(SelectedSlotWrapper(slot));
            slot++;
        }
    }

    UnityAction SelectedSlotWrapper(int slot)
    {
        return () => SelectSlot(slot);
    }

    void RefreshImages()
    {
        Sprite newSlotSprite;
        Color newSlotColor;
        for(int slot = 0; slot < transform.childCount; slot++)
        {
            GameObject storedGameObject = inventory.storedObjects[slot];
            if(storedGameObject != null)
            {
                newSlotSprite = storedGameObject.GetComponent<SpriteRenderer>().sprite;
                newSlotColor = new Color(1,1,1,1);
            } else
            {
                newSlotSprite = defaultSlotSprite;
                newSlotColor = defaultSlotColor;
            }
            Image slotImage = transform.GetChild(slot).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            slotImage.sprite = newSlotSprite;
            slotImage.color = newSlotColor;
        }
    }

    void SelectSlot(int slot)
    {
        Debug.Log("selected " + slot);
        inventory.ToggleSlot(slot);
    }
}

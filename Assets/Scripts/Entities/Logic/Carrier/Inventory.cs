using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    private Carrier carrier;
    private StatsScript statsScript;

    private int lastStoredSlot;

    public int maxStorage = 10;
    public GameObject[] storedObjects;
    private EventManager eventManager;

    public event Action InventoryUpdateEvent = delegate{};

    public void Awake()
    {
        storedObjects = new GameObject[maxStorage];
        carrier = GetComponent<Carrier>();
        statsScript = GetComponent<StatsScript>();
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    public void ToggleSlot(int slot)
    {
        if(storedObjects[slot] != null)
        {
            FillCarryFrom(slot);
        } else
        {
            StoreCarryTo(slot);
        }
    }

    public void SmartToggle()
    {
        if(carrier.isCarrying)
        {
            StoreToFirstOpenSlot();
        }
        else {
            if(storedObjects[lastStoredSlot] != null)
                FillFromLastStoredSlot();
            else
                FillFromFirstFullSlot();
        }
    }

    private void StoreToFirstOpenSlot()
    {
        for(int slot = 0; slot < storedObjects.Length; slot++)
        {
            if (storedObjects[slot] == null)
            {
                StoreCarryTo(slot);
                return;
            }
        }
    }

    private void FillFromFirstFullSlot()
    {
        for(int slot = 0; slot < storedObjects.Length; slot++)
        {
            if (storedObjects[slot] != null)
            {
                FillCarryFrom(slot);
                return;
            }
        }
    }

    private void FillFromLastStoredSlot()
    {
        FillCarryFrom(lastStoredSlot);
    }

    void FillCarryFrom(int slot)
    {
        GameObject storedObject = storedObjects[slot];
        carrier.ContinueCarry(storedObject);
        storedObjects[slot] = null;
        Debug.Log("fill carry " + slot);

        Carriable carriable = storedObject.GetComponent<Carriable>();
        statsScript.RemoveStatsChange(carriable.statsChangeOnStored);

        InventoryUpdateEvent();
    }

    void StoreCarryTo(int slot)
    {
        GameObject carriedObject = carrier.PauseCarry();
        if(carriedObject == null) return;

        carriedObject.SetActive(false);
        storedObjects[slot] = carriedObject;
        Debug.Log("Store carry " + slot);

        Carriable carriable = carriedObject.GetComponent<Carriable>();
        statsScript.AddStatsChange(carriable.statsChangeOnStored);

        eventManager.TriggerEvent(EventType.OnStore, gameObject);
        lastStoredSlot = slot;
        InventoryUpdateEvent();
    }
}

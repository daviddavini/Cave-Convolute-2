using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCarrierController : CarrierController
{
    private Joybutton carryButton;
    private Joybutton storeButton;

    private Inventory inventory;

    protected override void Awake()
    {
        base.Awake();
        carryButton = GameObject.FindWithTag("Carry Button").GetComponent<Joybutton>();
        carryButton.ButtonDownEvent += HandleCarryBySearch;

        inventory = GetComponent<Inventory>();

        storeButton = GameObject.FindWithTag("Store Button").GetComponent<Joybutton>();
        storeButton.ButtonDownEvent += inventory.SmartToggle;
    }
}

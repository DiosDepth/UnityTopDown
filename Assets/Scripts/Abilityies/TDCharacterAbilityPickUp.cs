using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class TDCharacterAbilityPickUp : TDCharacterAbility,EventListener<InventoryEvent>
{
    // Start is called before the first frame update
    public Heap<TDItemPicker> currentTouchItemPicker;
    public TDItemPicker currentPicker;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void InitialAbility()
    {
        base.InitialAbility();
        currentTouchItemPicker = new Heap<TDItemPicker>(10);
        this.StartListening<InventoryEvent>();
        InputSystemManager.instance.Gameplay_InputAction_Interaction += HandleInput;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
        if(currentTouchItemPicker.Count > 0)
        {
            
        }
    }

    public void OnEvent(InventoryEvent evt)
    {
        switch (evt.evttype)
        {
            case TDEnums.InventoryEventType.PickRequest:
                break;
            case TDEnums.InventoryEventType.ContentChanged:
               
                break;
            case TDEnums.InventoryEventType.ItemUseRequest:
                break;
            case TDEnums.InventoryEventType.Selected:
                break;
        }
    }

    public override void HandleInput(InputAction.CallbackContext callbackContext)
    {
        base.HandleInput(callbackContext);
        if(callbackContext.phase == InputActionPhase.Performed)
        {
            if(currentTouchItemPicker.Count > 0)
            {
                currentPicker = currentTouchItemPicker.GetFirst();
                //InventoryEvent.Trigger(TDEnums.InventoryEventType.PickRequest, pickup.inventoryItem.targetInventoryName, pickup.inventoryItem, 0);
                currentPicker.Pick(owner);
                
            }
        }
    }

    public void PickFaild()
    {

    }

    public void PickSuccess()
    {
        currentTouchItemPicker.RemoveFirst();
        currentPicker = null;
    }

    public override void OnRemove()
    {
        base.OnRemove();
        InputSystemManager.instance.Gameplay_InputAction_Pause -= HandleInput;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ItemPicker")
        {
            currentTouchItemPicker.Add(other.GetComponent<TDItemPicker>());
            Debug.Log("Enter Picker = " + other.name);
        }
        
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "ItemPicker")
        {
            if(!currentTouchItemPicker.Contains(other.GetComponent<TDItemPicker>()))
            {
                currentTouchItemPicker.Add(other.GetComponent<TDItemPicker>());
            }

            Debug.Log("Stay Picker = " + other.name);
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "ItemPicker")
        {
            currentTouchItemPicker.Remove(other.GetComponent<TDItemPicker>());
            Debug.Log("Exit Picker = " + other.name);
        }
    }
}

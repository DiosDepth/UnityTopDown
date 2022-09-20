using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TDCharacterAbilityHandleInventory : TDCharacterAbility
{
    

    public override void InitialAbility()
    {
        base.InitialAbility();
        ProcessInventory();
        InputSystemManager.instance.Gameplay_InputAction_Use += HandleInput;
    }

    public void ProcessInventory()
    {
        // (TDUIManager.instance.playerHUDCanvas.info as TDGUIPlayerHUD).inventoryHotBar.alpha = 1;
        //初始化所有的Inventory，设置他们的targetDisplayer
        TDGUIPlayerHUD playerhud = TDUIManager.instance.GetGUI<TDGUIPlayerHUD>("TDGUIPlayerHUD");
        TDInventoryDisplayer[] displayer = playerhud.GetComponentsInChildren<TDInventoryDisplayer>();

        foreach(KeyValuePair<string,TDInventory> pair in TDInventoryManager.instance.inventoryDic)
        {
            for (int i = 0; i < displayer.Length; i++)
            {
                if (pair.Value.inventoryName == displayer[i].targetInventoryName)
                {
                    pair.Value.owner = GetComponent<TDCharacter>();
                    pair.Value.SetTargetDisplayer(displayer[i]);
                    displayer[i].targetInventory = pair.Value;
                    displayer[i].Initialization();
                }
            }
            
        }



    }
    public override void HandleInput(InputAction.CallbackContext callbackContext)
    {
        base.HandleInput(callbackContext);
        if(callbackContext.phase == InputActionPhase.Performed)
        {
            TDInventoryManager.instance.currentSelectedSlot.Use();
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        InputSystemManager.instance.Gameplay_InputAction_Use -= HandleInput;
    }
}

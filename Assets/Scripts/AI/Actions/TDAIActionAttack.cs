using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDAIActionAttack : TDAIAction
{
    public float attackInterval = 1f;
    private TDCharacterAbilityHandleWeapon handle;
    public override void Initialization()
    {
        base.Initialization();
        handle = GetComponent<TDCharacterAbilityHandleWeapon>();
    }


    public override void OnEnterAction()
    {
        base.OnEnterAction();
    }

    public override void UpdateAction()
    {

        handle.WeaponFire();
        Debug.Log("Attacking");
    }

    public override void OnExitAction()
    {
        base.OnExitAction();
    }
}

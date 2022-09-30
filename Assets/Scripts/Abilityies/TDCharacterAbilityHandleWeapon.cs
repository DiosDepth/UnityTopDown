using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TDEnums;
using InputSpace;


namespace TDEnums
{
    public enum HandleWeaponState
    {
        Waitting,
        OnTrigger,
        Charging,
        ChargingCompleted,
        Fire,
        ChargingFire,
    }
}


[RequireComponent(typeof(TDCharacter))]
public class TDCharacterAbilityHandleWeapon : TDCharacterAbility
{
    public Transform mainWeaponSlot;
    public Transform mainWeaponRoot;
    public AvalibleWeapons InitialWeapon;
    public TDWeapon currentMainWeapon;
    public TDWeapon currentSecondWeapon;
    public TDStateMachine<HandleWeaponState> handleState;

    public Vector3 _aimingDirection;


   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitialAbility()
    {
        owner = this.GetComponent<TDCharacter>();
        
        mainWeaponSlot = this.gameObject.transform.Find("WeaponSlot");
        
        if (mainWeaponSlot == null)
        {
            if (isShowDebug)
            {
                Debug.Log("TDCharacterAbilityHandleWeapon : " + "can't find weapon slot in this object");
            }

            return;
        }
        

        if (InitialWeapon == AvalibleWeapons.None)
        {
            if(isShowDebug)
            {
                Debug.LogWarning("TDCharacterAbilityHandleWeapon : " + "InitialWeapon set to none");
            }
            return;
        }
        
        if (currentMainWeapon == null)
        {
            string temppath = TDDataManager.instance.GetWeaponDataInfo(InitialWeapon).PrefabPath;
            TDDataManager.instance.LoadResAsync<GameObject>(temppath, (obj) =>
            {
                obj.transform.parent = mainWeaponSlot;
                mainWeaponRoot = obj.transform;
                obj.transform.localPosition = Vector3.zero;
                currentMainWeapon = obj.GetComponent<TDWeapon>();
                //Instantiate(obj, mainweaponSlot);
                currentMainWeapon.SetOwner(owner);
                currentMainWeapon.Initialization();
                //Assin weapon animator to TDcharacter 
                BindAnimator();
                currentMainWeapon.WeaponON();
                handleState = new TDStateMachine<HandleWeaponState>(this.gameObject);
                if (handleState.currentState != HandleWeaponState.Waitting)
                {
                    handleState.ChangeState(HandleWeaponState.Waitting);
                }
            }
            );
        }
        

        InputSystemManager.instance.Gameplay_InputAction_Attack += HandleInput;
        InputSystemManager.instance.GamePlay_InputAction_Aiming += HandleAimingInput;

    }
    public override void HandleInput(InputAction.CallbackContext callbackContext)
    {
        if(owner.characterType != TDEnums.CharacterType.Player)
        {
            return;
        }
        base.HandleInput(callbackContext);
        //按键按下
        if(callbackContext.phase == InputActionPhase.Started)
        {
            WeaponOnTrigger();
        }

        if(callbackContext.phase == InputActionPhase.Performed)
        {
 
            WeaponChargeStart();
        }
        ////完成了HoldOn
        //if (callbackContext.phase == InputActionPhase.Performed)
        //{
        //    if (currentMainWeapon.isStartCharging == false)
        //    {
        //        if (Time.time - currentMainWeapon.ChargeTimeStamp >= currentMainWeapon.StartChargingTime)
        //        {
        //            currentMainWeapon.isStartCharging = true;
        //        }
        //    }
        //    if (currentMainWeapon.isStartCharging == true)
        //    {
        //        if (Time.time - currentMainWeapon.ChargeTimeStamp >= currentMainWeapon.CompletedChargeTime)
        //        {
        //            currentMainWeapon.isChargingCompleted = true;
        //            WeaponChargeCompleted();
        //        }
        //        else
        //        {
        //            WeaponCharging();
        //        }
        //    }
        //}
        //放开按键开火
        if (callbackContext.phase == InputActionPhase.Canceled)
        {
            if (currentMainWeapon.isChargingCompleted)
            {
                WeaponChargeFire();
            }
            else
            {
                WeaponFire();
            }
        }
    }

    public void HandleAimingInput(InputAction.CallbackContext callbackContext)
    {
        if(owner.characterType != CharacterType.Player)
        {
            return;
        }
       if (InputSystemManager.instance.playerInput.currentControlScheme == inputControlSchemeType.PlayerKeyBoard.ToString())
        {
  
            _aimingDirection = TDCameraManager.instance.currentActiveCamera.ScreenToWorldPoint(callbackContext.ReadValue<Vector2>());
            _aimingDirection = new Vector3(_aimingDirection.x - mainWeaponRoot.position.x, _aimingDirection.y - mainWeaponRoot.position.y, 0);
            _aimingDirection = _aimingDirection.normalized;


        }
       if(InputSystemManager.instance.playerInput.currentControlScheme == inputControlSchemeType.PlayerGamePad.ToString())
        {
            _aimingDirection = callbackContext.ReadValue<Vector2>();
            _aimingDirection.z = 0;
            _aimingDirection = _aimingDirection.normalized;
        }
    }

    public override void EarlyUpdateAbility()
    {
        base.EarlyUpdateAbility();
        Aiming();
        
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
        //处理武器的蓄力攻击
        if(handleState == null)
        {
            Debug.LogWarning("handleState is null, please initial weapon before using");
            return;
        }
        if (handleState?.currentState == HandleWeaponState.Charging)
        {
                if (Time.time - currentMainWeapon.ChargeTimeStamp >= currentMainWeapon.CompletedChargeTime)
                {
                    
                    WeaponChargeCompleted();
                }
                else
                {
                    WeaponCharging();
                }
        }


    }

    //玩家按下按键的瞬间
    public virtual void WeaponOnTrigger()
    {
        currentMainWeapon.isStartCharging = false;
        currentMainWeapon.isChargingCompleted = false;
        handleState.ChangeState(HandleWeaponState.OnTrigger);

    }

    public virtual void WeaponChargeStart()
    {
        if (!isAbilityOn
            || owner.conditionState.currentState != TDEnums.CharacterStates.CharacterConditions.Normal
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Attack
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Chargeing
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Dash
            || currentMainWeapon.weaponStates.currentState != TDEnums.WeaponStates.WeaponIdle)
        {
            return;
        }

        if (handleState.currentState == HandleWeaponState.OnTrigger)
        {
            currentMainWeapon.ChargeTimeStamp = Time.time;
            handleState.ChangeState(HandleWeaponState.Charging);
            Debug.Log("TDCharacterAbilityHandleWeapon : " + "WeaponChargeStart!");
        }
            
          
    }

    public virtual void WeaponCharging()
    {
        owner.GetComponent<TDCharacter>().movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Chargeing);
        handleState.ChangeState(HandleWeaponState.Charging);
        Debug.Log("TDCharacterAbilityHandleWeapon : " + "WeaponCharging!");
        
    }

    public virtual void WeaponFire()
    {
        if(!isAbilityOn
            || owner.conditionState.currentState != TDEnums.CharacterStates.CharacterConditions.Normal
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Attack
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Charged
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Dash
            || currentMainWeapon.weaponStates.currentState != TDEnums.WeaponStates.WeaponIdle)
        {
            return;
        }
      
        owner.GetComponent<TDCharacter>().movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Attack);
        Debug.Log("TDCharacterAbilityHandleWeapon : " + "WeaponFire!");
        currentMainWeapon.WeaponInputStart();
    }

    public virtual void WeaponChargeFire()
    {
        if (!isAbilityOn
            || owner.conditionState.currentState != TDEnums.CharacterStates.CharacterConditions.Normal
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Attack
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Chargeing
            || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Dash
            || currentMainWeapon.weaponStates.currentState != TDEnums.WeaponStates.WeaponIdle)
        {
            return;
        }
    
        owner.GetComponent<TDCharacter>().movementState.ChangeState(TDEnums.CharacterStates.MovementStates.ChargeingAttack);
        Debug.Log("TDCharacterAbilityHandleWeapon : " + "WeaponChargeFire!");
        currentMainWeapon.WeaponInputStart();
    }

    public virtual void WeaponChargeCompleted()
    {
        currentMainWeapon.isChargingCompleted = true;
        owner.GetComponent<TDCharacter>().movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Charged);
        handleState.ChangeState(HandleWeaponState.ChargingCompleted);
        Debug.Log("TDCharacterAbilityHandleWeapon : " + "WeaponChargeCompleted!");
    }

    public virtual void WeaponAttakEnd()
    {
        owner.GetComponent<TDCharacter>().movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Idle);
        Debug.Log("TDCharacterAbilityHandleWeapon : " + "WeaponAttackEnd!");
    }

    public virtual void Aiming()
    {
        if(currentMainWeapon == null)
        {
            return;
        }
        Debug.DrawLine(mainWeaponRoot.position, mainWeaponRoot.position+_aimingDirection*5f , Color.green, 0.1f);
        ExtensionMathTools.ApplyRotationWithDirectionXY(mainWeaponRoot, _aimingDirection, 720);
        
    }

    protected override void InitializeAnimatorParameters()
    {

    }

    public override void UpdateAnimators()
    {
    }

    private void UpdateWeaponSlotDirection()
    {
        
    }
    public override void OnRemove()
    {
        base.OnRemove();
        InputSystemManager.instance.Gameplay_InputAction_Attack -= HandleInput;
        InputSystemManager.instance.GamePlay_InputAction_Aiming -= HandleAimingInput;
    }
}

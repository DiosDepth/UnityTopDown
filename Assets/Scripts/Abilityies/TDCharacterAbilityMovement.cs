using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementType
{
    NonPhysical,
    Physicals
}

[RequireComponent(typeof(TDCharacterController))]
public class TDCharacterAbilityMovement : TDCharacterAbility
{
    [Header("---MovementSettings---")]
    public MovementType movementType = MovementType.NonPhysical;
    public float maxMoveSpeed = 10;
    public float acceleration = 10;
    public bool isFlipCharacter = true;
    public float rotSpeed = 10f;
    public Transform rotationRoot;
    public bool movementForbiden = false;
    public bool rotationForbiden = false;
    [Header("---MovementInfo---")]
    
    public Vector3 inputDirection;
    public Vector3 lerpedInput;
    public Vector3 deltaMovement;
    public float deltaSpeed;
    public float deltaAcceleration;
    /// <summary>
    /// normalized face direction
    /// </summary>
    public Vector3 faceDirection;
   

    protected TDCharacterController _tdCharacterController;
    // Start is called before the first frame update


 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    public Vector3 GetMoveDirection()
    {
        Vector3 tempdir = Vector3.zero;
        tempdir = inputDirection;
        return tempdir;
    }

    public void SetMoveDirection( Vector3 dir)
    {
        inputDirection = dir;
    }

    public void FlipCharacter()
    {
        if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Dash ||
        owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill ||
        owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.ChargeingAttack)
        {
            return;
        }
        if (owner.conditionState.currentState != TDEnums.CharacterStates.CharacterConditions.Normal)
        {
            return;
        }
        _tdCharacterController.FlipWithMovdingDirection(rotationRoot, inputDirection);
    }

    public override void InitialAbility()
    {
        base.InitialAbility();
        _tdCharacterController = GetComponent<TDCharacterController>();
        movementForbiden = false;
        InputSystemManager.instance.Gameplay_InputAction_Movement += HandleInput;
    }

    public void HandleMovement(InputAction.CallbackContext callbackContext)
    {
        if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Dash ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.ChargeingAttack)
        {
            inputDirection.x = 0;
            inputDirection.y = 0;
            return;
        }
        if (owner.conditionState.currentState != TDEnums.CharacterStates.CharacterConditions.Normal)
        {
            inputDirection.x = 0;
            inputDirection.y = 0;
            return;
        }

        inputDirection.x = callbackContext.ReadValue<Vector2>().x;
        inputDirection.y = callbackContext.ReadValue<Vector2>().y;

    }

    public override void HandleInput(InputAction.CallbackContext callbackContext)
    {
        base.HandleInput(callbackContext);
        if (movementForbiden)
        {
            inputDirection = Vector3.zero;
            return;
        }
        if (owner.characterType == TDEnums.CharacterType.Player)
        {
            HandleMovement(callbackContext);
        }
        if(owner.characterType == TDEnums.CharacterType.AI)
        {
            //moveDirection = Vector3.zero;
        }
    }

    public override void EarlyUpdateAbility()
    {
        base.EarlyUpdateAbility();
        
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
        if(movementType == MovementType.NonPhysical)
        {
            CharacterMovement(movementType);
        }




    }

    public override void AfterUpdateAbility()
    {
        base.AfterUpdateAbility();
        if (movementType == MovementType.Physicals)
        {
            CharacterMovement(movementType);
        }
    }

    public override void EarlyFixedUpdateAbility()
    {
        base.EarlyFixedUpdateAbility();
    }

    public override void FixedUpdateAbility()
    {

        
    }
    public override void OnRemove()
    {
        base.OnRemove();
        InputSystemManager.instance.Gameplay_InputAction_Movement -= HandleInput;
    }

    private void CharacterMovement(MovementType m_movement_type)
    {
        base.FixedUpdateAbility();
        if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Attack ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Chargeing ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.ChargeingAttack ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Charged)
        {
            return;
        }

        if (owner.conditionState.currentState == TDEnums.CharacterStates.CharacterConditions.Dead ||
           owner.conditionState.currentState == TDEnums.CharacterStates.CharacterConditions.Frozen ||
           owner.conditionState.currentState == TDEnums.CharacterStates.CharacterConditions.Immobilized ||
           owner.conditionState.currentState == TDEnums.CharacterStates.CharacterConditions.ShutDown ||
           owner.conditionState.currentState == TDEnums.CharacterStates.CharacterConditions.Paused)
        {
            return;
        }

        if (inputDirection.sqrMagnitude!=0)
        {
            deltaAcceleration = Mathf.Lerp(deltaAcceleration, 1, acceleration * Time.deltaTime);
            lerpedInput = Vector2.ClampMagnitude(inputDirection, deltaAcceleration);
        }
        else
        {
            deltaAcceleration = Mathf.Lerp(deltaAcceleration, 0, acceleration * Time.deltaTime);
            lerpedInput = Vector2.Lerp(lerpedInput, lerpedInput * deltaAcceleration, acceleration * Time.deltaTime);
 
        }

        deltaMovement = lerpedInput;
        deltaSpeed = Mathf.Lerp(deltaSpeed, maxMoveSpeed, deltaAcceleration * Time.deltaTime);
        deltaMovement *= deltaSpeed;

        if (deltaMovement.magnitude > maxMoveSpeed)
        {
            deltaMovement = Vector3.ClampMagnitude(deltaMovement, maxMoveSpeed);
        }

        if (deltaMovement.sqrMagnitude > 0)
        {
            _tdCharacterController.ApplyMovement(deltaMovement);// move character
            if (isFlipCharacter)// rotate character if true
            {
                if (_tdCharacterController.IsMoving())// see if character is moving 
                {
                    FlipCharacter();//  Flip Charater 
                }
            }
            if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Idle || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Null)
            {
                owner.movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Move);
            }
        }
        else
        {
            _tdCharacterController.StopMovement();
            if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Move || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Null)
            {
                owner.movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Idle);
            }
        }
    }

    
    public override void AfterFixedUpdateAbility()
    {
        base.AfterFixedUpdateAbility();
        if (isShowDebug)
        {
            _tdCharacterController.DebugVelocity();
        }
    }
}

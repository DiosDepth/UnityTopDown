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
    public Vector3 moveDirection;
    public Vector3 deltaMovement;
    public float deltaSpeed;
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

        if (callbackContext.performed)
        {
            moveDirection = inputDirection;
        }

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
    }

    public override void EarlyFixedUpdateAbility()
    {
        base.EarlyFixedUpdateAbility();
    }

    public override void FixedUpdateAbility()
    {
        if(movementType == MovementType.Physicals)
        {
            CharacterMovement(movementType);
        }
        
    }
    public override void OnRemove()
    {
        base.OnRemove();
        InputSystemManager.instance.Gameplay_InputAction_Movement -= HandleInput;
    }

    private void CharacterMovement(MovementType m_movement_type)
    {
        if(inputDirection.sqrMagnitude!=0)
        {
            deltaSpeed = _tdCharacterController.CalculateDeltaMoveSpeed(deltaSpeed, acceleration, maxMoveSpeed);
            if (Mathf.Approximately(deltaSpeed, maxMoveSpeed))
            {
                deltaSpeed = maxMoveSpeed;
            }
            deltaMovement = deltaSpeed * moveDirection;
        }
        else
        {
            deltaSpeed = _tdCharacterController.CalculateDeltaMoveSpeed(deltaSpeed, -1* acceleration, maxMoveSpeed);
            if(Mathf.Approximately(deltaSpeed,0))
            {
                deltaSpeed = 0;
            }
            deltaMovement = deltaSpeed * moveDirection;

        }
        
        base.FixedUpdateAbility();
        if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Skill ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Attack ||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Chargeing||
            owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.ChargeingAttack||
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



        if (deltaMovement.sqrMagnitude > 0)
        {
            _tdCharacterController.ApplyMovement(m_movement_type, deltaMovement);// move character
            if (isFlipCharacter)// rotate character if true
            {
                if (_tdCharacterController.IsMoving(m_movement_type, deltaMovement))// see if character is moving 
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
            _tdCharacterController.StopMovement(m_movement_type);
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

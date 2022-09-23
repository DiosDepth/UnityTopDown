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
    public float thresholdMoveSpeed = 0.01f;
    public bool isFlipCharacter = true;

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

    [Header("---Animation---")]
    public AnimationParamInfo animParam_walk;
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


    }

    public override void AfterUpdateAbility()
    {
        base.AfterUpdateAbility();

        CharacterMovement(movementType);
        
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
        if (movementForbiden)
        {
            return;
        }

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


        //计算当前帧的加速度比例并且对操作输入进行限制, 让其在当前帧内的Magnitude小于或者等于当前帧的加速比例

        if (inputDirection.sqrMagnitude!=0)//当操作输入不为0 也就是有操作输入进来的时候 , 做加速运动
        {
            deltaAcceleration = Mathf.Lerp(deltaAcceleration, 1, acceleration * Time.deltaTime);//这里计算当前帧的加速度比例, 比如每秒的加速度是acceleration, 当前的加速度会从0开始到1 按照这个比例进行计算, 每一帧积累会无限接近于1
            if (Mathf.Approximately(deltaAcceleration, 1))
            {
                deltaAcceleration = 1;
            }
            lerpedInput = Vector2.ClampMagnitude(inputDirection, deltaAcceleration);
        }
        else
        {
            deltaAcceleration = Mathf.Lerp(deltaAcceleration, 0, acceleration * Time.deltaTime);
            if(Mathf.Approximately(deltaAcceleration,0))
            {
                deltaAcceleration = 0;
            }
            if (deltaAcceleration == 0)
            {
                lerpedInput = Vector3.zero;
            }
            else
            {
                lerpedInput = Vector2.Lerp(lerpedInput, lerpedInput * deltaAcceleration, acceleration * Time.deltaTime);
            }
          
 
        }

        deltaMovement = lerpedInput;
        deltaSpeed = Mathf.Lerp(0, maxMoveSpeed, deltaAcceleration);//根据当前帧的加速比例 计算当前帧的移动速度
        deltaMovement *= deltaSpeed;

        //限制最大移动速度
        if (deltaMovement.magnitude > maxMoveSpeed)
        {
            deltaMovement = Vector3.ClampMagnitude(deltaMovement, maxMoveSpeed);
        }

        if (deltaMovement.sqrMagnitude > Mathf.Pow( thresholdMoveSpeed,2))
        {
            _tdCharacterController.ApplyMovement(deltaMovement);// Set movement in controller to move 
            if (isFlipCharacter)// rotate character if true
            {
                if (IsMovingByInput())// see if character is moving 
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
            deltaSpeed = 0;
            deltaMovement = Vector3.zero;
            _tdCharacterController.StopMovement();
            if (owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Move || owner.movementState.currentState == TDEnums.CharacterStates.MovementStates.Null)
            {
                owner.movementState.ChangeState(TDEnums.CharacterStates.MovementStates.Idle);
            }
        }
    }

    public bool IsMovingByInput()
    {
        return inputDirection.sqrMagnitude != 0;
    }

    
    public override void AfterFixedUpdateAbility()
    {
        base.AfterFixedUpdateAbility();
        if (isShowDebug)
        {
            _tdCharacterController.DebugVelocity();
        }
    }


    protected override void InitializeAnimatorParameters()
    {
        RegisterAnimatorParameter(animParam_walk);

    }
    public override void UpdateAnimators()
    {
        base.UpdateAnimators();
        owner.characterAnimator.UpdateAnimationParamBool(animParam_walk.paramaterName, IsMovingByInput(), owner.characterAnimatorParameters);
   
    }
}
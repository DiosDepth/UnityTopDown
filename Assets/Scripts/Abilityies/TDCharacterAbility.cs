using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;
using UnityEngine.InputSystem;

[System.Serializable]
public struct AnimationParamInfo
{
    public string paramaterName;
    public AnimatorControllerParameterType paramaterType;
}



[System.Serializable]
public class TDCharacterAbility : MonoBehaviour
{
    
    //Consume AP 
    public float apConsume = 0;
    public float cdTime = 0;
    public bool isFixedUpdate;
    /// <summary>
    /// flag for skill and basic ability : false is basic ability, true is skill that needs trigger to use,
    /// </summary>
    public bool isSkill = false;
    /// <summary>
    /// unique skill name to define what skill is, it must be unique
    /// </summary>
    public string uniqueSkillName;

    public bool isAbilityOn = true;
    public TDCharacter owner;
    public Animator animator;

    public bool isShowDebug = false;

    protected bool _abilityInitialized = false;
    protected float _verticalInput;
    protected float _horizontalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void InternalHandleInput()
    {
        if(TDInputManager.instance == null)
        {
            return;
        }
        _verticalInput = TDInputManager.instance.inputMovement.z;
        _horizontalInput = TDInputManager.instance.inputMovement.x;
        //HandleInput();
    }

    public virtual void HandleInput(InputAction.CallbackContext callbackContext)
    {

    }

 

    public virtual void InitialAbility()
    {
        owner = this.GetComponent<TDCharacter>();
        BindAnimator();

    }

    public virtual void EarlyUpdateAbility()
    {
        if (!isAbilityOn)
        {
            return;
        }
        InternalHandleInput();
    }

    public virtual void UpdateAbility()
    {
        if (!isAbilityOn)
        {
            return;
        }
    }

    public virtual void AfterUpdateAbility()
    {
        if (!isAbilityOn)
        {
            return;
        }
    }


    public virtual void EarlyFixedUpdateAbility()
    {
        if (!isAbilityOn)
        {
            return;
        }
    }

    public virtual void FixedUpdateAbility()
    {
        if (!isAbilityOn)
        {
            return;
        }
    }

    public virtual void AfterFixedUpdateAbility()
    {
        if (!isAbilityOn)
        {
            return;
        }
    }

    public virtual void OnRemove()
    {
        if(!isAbilityOn)
        {
            return;
        }
    }
    protected virtual void BindAnimator()
    {
        animator = owner.characterAnimator;
        if (animator != null)
        {
            InitializeAnimatorParameters();
        }
    }

    //initial animator parameters about abilities, this is link with character animator, all abilities animator paramater will registed in the character's animator parameters
    protected virtual void InitializeAnimatorParameters()
    {

    }

    //update ability animator, which is character animator, you should see BindAnimator() where bind animator to owner.characterAnimator which is character's animator
    public virtual void UpdateAnimators()
    {
    }

    protected virtual void RegisterAnimatorParameter(AnimationParamInfo m_paraminfo)
    {
        if (animator == null)
        {
            return;
        }
        if (animator.HasParameterOfType(m_paraminfo.paramaterName, m_paraminfo.paramaterType))
        {
            owner.characterAnimatorParameters.Add(m_paraminfo.paramaterName);
        }
    }


}

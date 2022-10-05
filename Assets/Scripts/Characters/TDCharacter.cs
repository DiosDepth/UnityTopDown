using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;

namespace TDEnums
{
    public enum CharacterType
    {
        Player,
        AI
    }

    public class CharacterStates
    {
        public enum CharacterConditions
        {
            Normal,
            Frozen,
            ShutDown,
            Paused,
            Immobilized,
            Dead,
        }

        public enum MovementStates
        {
            Null,
            Idle,
            Move,
            Attack,
            Chargeing,
            Charged,
            ChargeingAttack,
            Skill,
            Dash,
        }

        public enum AIDicisionStates
        {
            Idle,
            Partol,
            Chasing,
        }
    }
}

public class TDCharacter : MonoBehaviour
{
    public bool isShowDebug;
    public CharacterType characterType = CharacterType.Player;
    public TDStateMachine<CharacterStates.MovementStates> movementState;
    public TDStateMachine<CharacterStates.CharacterConditions> conditionState;
    public Transform characterModleContainer;

    public Animator characterAnimator;
    public List<string> characterAnimatorParameters;


    [SerializeField]
    public TDCharacterAbility[] abilityLists;
    public Dictionary<string, int> abilityDic = new Dictionary<string, int>();
    public TDCharacterController controller;
    [SerializeField]
    public TDCharacterAttribute attribute;

    public bool isUpdateAbility = false;
    protected int abilityCount;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {

    }





    public void Initialization()
    {
        controller = GetComponent<TDCharacterController>();
        abilityLists = GetComponents<TDCharacterAbility>();

        for (int i = 0; i < abilityLists.Length ; i++)
        {
            abilityDic.Add(abilityLists[i].uniqueSkillName, i);
        }
        abilityCount = abilityLists.Length;
        isUpdateAbility = false;
        movementState = new TDStateMachine<CharacterStates.MovementStates>(this.gameObject);
        conditionState = new TDStateMachine<CharacterStates.CharacterConditions>(this.gameObject);
        characterModleContainer = transform.Find("ModelContainer");

        AssignCharacterAnimator();
        InitialAbility();


        movementState.triggerDebugLog = isShowDebug;
        conditionState.triggerDebugLog = isShowDebug;
        
        //Debug.Log("MovementState : " + movementState.currentState);
        //Debug.Log("ConditionState : " + conditionState.currentState);
    }


    public virtual TDCharacterAbility GetAbilityByUniqueSkillName(string m_skillname)
    {
        int index = abilityDic[m_skillname];
        return abilityLists[index];
    }

    public virtual void AssignCharacterAnimator()
    {
        if (characterAnimator == null)
        {
            characterAnimator = GetComponentInChildren<Animator>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isUpdateAbility)
        {
            EveryFrame();
        }
  
    }

    private void FixedUpdate()
    {
        if(isUpdateAbility)
        {
            EveryFixedFrame();
        }
 
    }
    public virtual void EveryFrame()
    {
        EarlyUpdateAbility();
        UpdateAbility();
        AfterUpdateAbility();

        UpdateAnimator();
    }

    public virtual void EveryFixedFrame()
    {
        EarlyFixedUpdateAbility();
        FixedUpdateAbility();
        AfterFixedUpdateAbility();
    }


    public virtual void InitialAbility()
    {
        
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].InitialAbility();
        }
    }



    public virtual void EarlyUpdateAbility()
    {
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].EarlyUpdateAbility();
        }
    }

    public virtual void UpdateAbility()
    {
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].UpdateAbility();
        }
    }

    public virtual void AfterUpdateAbility()
    {
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].AfterUpdateAbility();
        }
    }

    public virtual void EarlyFixedUpdateAbility()
    {
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].EarlyFixedUpdateAbility();
        }
    }

    public virtual void FixedUpdateAbility()
    {
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].FixedUpdateAbility();
        }
    }

    public virtual void AfterFixedUpdateAbility()
    {
        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].AfterFixedUpdateAbility();
        }
    }
    // initialize character animator parameters
    protected virtual void InitializeAnimatorParameters()
    {

    }
    //update character animator, other than weapon animator
    public virtual void UpdateAnimator()
    {

        for (int i = 0; i < abilityCount; i++)
        {
            abilityLists[i].UpdateAnimators();
        }
    }


}

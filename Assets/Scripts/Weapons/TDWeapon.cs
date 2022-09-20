using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;

namespace TDEnums
{
    public enum AimType
    {
        TargetBase,
        DirectionBase,
    }
    public enum WeaponType
    {
        Instante,
        Projectile,
        Melee,

    }
    public enum WeaponShottingType
    {
        HG,
        SG,
        SMG,
        AR,
    }
    public enum WeaponStates
    {
        Null,
        WeaponIdle,
        WeaponStart,
        WeaponDelayBeforeUse,
        WeaponUse,
        WeaponDelayBetweenUses,
        WeaponStop,
        WeaponInterrupted
    }

    public enum WeaponDamageStates
    {
        BeforeDamage,
        DuringDamage,
        AfterDamage,
        EndDamage
    }

}

public struct TDWeaponDamageEvent
{
    public WeaponDamageStates damageState;
    public TDCharacter target;
    public TDCharacter caster;
    public TDWeaponDamageEvent(WeaponDamageStates m_state, TDCharacter m_target, TDCharacter m_caster)
    {
        damageState = m_state;
        target = m_target;
        caster = m_caster;
    }
    static TDWeaponDamageEvent e;
    public static void Trigger(WeaponDamageStates m_state, TDCharacter m_target, TDCharacter m_caster)
    {
        e.damageState = m_state;
        e.target = m_target;
        e.caster = m_caster;
        TDEventManager.TriggerEvent<TDWeaponDamageEvent>(e);
    }
}


public class TDWeapon : MonoBehaviour
{
    [Header("---WeaponSettings---")]
    public WeaponStates state;
    public TDStateMachine<WeaponStates> weaponStates;
    public WeaponType weaponType = WeaponType.Melee;
    public AimType aimType = AimType.TargetBase;

    public float timeBetweenFire = 1;
    public float timeBeforeFire = 0;


    protected float _timeBetweenFireCounter;
    protected float _timeBeforeFireCounter;


    [Header("Charging")]
    public bool isStartCharging = false;
    public bool isChargingCompleted = false;
    //Charging Time
    public float CompletedChargeTime = 1f;
    public float ChargeTimeStamp;
    ///蓄力攻击消耗
    public bool APConsumer;
    public int APConsumeValue;

    [Header("Instante Weapon Damage Settings")]
    public float initialDamageDelay =0.3f;
    public float activeDamageDuration = 0.3f;
    public float afterDamageDelay = 0.4f;

    public LayerMask damageMask = 1 << 9;
    public Vector3 damageAreaSize = new Vector3(1,1,1);
    public Vector3 damageAreaOffset = new Vector3(0,0,1);
    public bool showDamageArea = false;


    [Header("Projectile Weapon Damage Settings")]
    public WeaponShottingType shottingType = WeaponShottingType.SG;
    public float totalDamage = 20;
    public int bulletCount = 3;
    

    [Header("WeaponAttribute")]
    public float weaponDamageValue = 20;
    public float criticalDamageRate = 0.1f;
    public float criticalDamageMultipler = 2;
    public float flashDamageMultipler = 5;

    private GameObject _damageArea ;
    private Collider[] damagedCollider = new Collider[50];


    [Header("Animation")]
    public Animator weaponAnimator;
    public List<string> weaponAnimatorParameters;
    [Header("Animation Parameters Names")]
    public string IdleAnimationParameter;
    public string StartAnimationParameter;
    public string ChargingStartAnimationParameter;
    public string DelayBeforeUseAnimationParameter;
    public string UseAnimationParameter;
    public string DelayBetweenUsesAnimationParameter;
    public string StopAnimationParameter;

    [HideInInspector]
    public TDCharacter owner;
    protected TDCharacterAbilityMovement _movement;
    protected TDCharacterAbilityHandleWeapon _weaponHandler;

    protected bool _isMeleeAttacking = false;
    // Start is called before the first frame update
    private bool isUpdate = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isUpdate)
        {
            UpdateAnimators();
        }

    }

    private void LateUpdate()
    {
        if(isUpdate)
        {
            ProcessWeaponState();
            state = weaponStates.currentState;
        }

    }

    public virtual void Initialization()
    {
        //base.Initialization();
        weaponAnimator = GetComponent<Animator>();
        _movement = owner.transform.GetComponent<TDCharacterAbilityMovement>();
        _weaponHandler = owner.transform.GetComponent<TDCharacterAbilityHandleWeapon>();
        weaponStates = new TDStateMachine<WeaponStates>(this.gameObject);
        InitializeAnimatorParameters();
        weaponStates.ChangeState(WeaponStates.WeaponIdle);

    }

    public virtual void CreateDamageArea()
    {
        _damageArea = new GameObject();
        _damageArea.name = this.gameObject.name + "DamageArea";
        _damageArea.transform.position = this.transform.position;
        _damageArea.transform.parent = this.transform;

        _damageArea.AddComponent<Collider>();

        

    }
    //TODO create a damage area
        //Add TouchOnDamage to a damage area 
    //TODO Damage area ON

    public virtual void MeleeWeaponDamageAreaON()
    {
        TDCharacterAbilityHealth temp_health;
        damagedCollider = Physics.OverlapBox(_movement.rotationRoot.TransformPoint(damageAreaOffset), damageAreaSize, Quaternion.identity, owner.attribute.damageMask);
        
        if(damagedCollider.Length >0 )
        {
            showDamageArea = true;
        }
        else
        {
            showDamageArea = false;
        }

        for (int i = 0; i < damagedCollider.Length; i++)
        {
            Debug.Log("Damage : " + damagedCollider[i].name);
            temp_health = damagedCollider[i].GetComponent<TDCharacterAbilityHealth>();
            temp_health.TakeDamage(owner.attribute.finalWeaponDamage,owner.gameObject,damagedCollider[i].gameObject);
        }
    }
    //TODO Damage area OFF
    private void OnDrawGizmos()
    {
        if(showDamageArea)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_movement.rotationRoot.TransformPoint(damageAreaOffset), damageAreaSize);
            showDamageArea = false; 
        }

    }

    public virtual void SetOwner(TDCharacter new_owner)
    {
        owner = new_owner;

    }

    protected virtual void ProcessWeaponState()
    {
        if(weaponStates == null)
        {
            return;
        }
        switch (weaponStates.currentState)
        {
            case WeaponStates.WeaponIdle:
                CaseWeaponIdle();
                break;

            case WeaponStates.WeaponStart:
                CaseWeaponStart();
                break;

            case WeaponStates.WeaponDelayBeforeUse:
                CaseWeaponDelayBeforeFire();
                break;

            case WeaponStates.WeaponUse:
                CaseWeaponFire();
                break;

            case WeaponStates.WeaponDelayBetweenUses:
                CaseWeaponDelayBetweenFire();
                break;

            case WeaponStates.WeaponStop:
                CaseWeaponStop();
                break;

            case WeaponStates.WeaponInterrupted:
                CaseWeaponInterrupted();
                break;
        }
    }

    protected virtual void CaseWeaponIdle()
    {
        //todo reset player movement speed
    }

    protected virtual void CaseWeaponStart()
    {
        if(timeBeforeFire>0)
        {
            _timeBeforeFireCounter = timeBeforeFire;
            weaponStates.ChangeState(WeaponStates.WeaponDelayBeforeUse);
        }
        else
        {
            FireRequest();
        }
    }

    protected virtual void CaseWeaponDelayBeforeFire()
    {
        _timeBeforeFireCounter -= Time.deltaTime;
        if(_timeBeforeFireCounter <=0)
        {
            FireRequest();
        }
    }


    protected virtual void CaseWeaponFire()
    {/* switch(weaponType)
        {
            case WeaponType.Projectile:
                ProjectileWeaponFire();
                break;
            case WeaponType.Melee:
                StartCoroutine(MeleeWeaoponFire());
                break;
        }*/
        StartCoroutine(MeleeWeaoponFire());


    }

    protected virtual void CaseWeaponDelayBetweenFire()
    {
        _timeBetweenFireCounter -= Time.deltaTime;
        if(_timeBetweenFireCounter <= 0)
        {
            WeaponTriggerOFF();
        }
    }


    protected virtual void CaseWeaponInterrupted()
    {
       
    }

    protected virtual void CaseWeaponStop()
    {
        weaponStates.ChangeState(WeaponStates.WeaponIdle);
  
        StopCoroutine(MeleeWeaoponFire());
        _weaponHandler.WeaponAttakEnd();
    }



    public virtual void WeaponInputStart()
    {
        if (weaponStates.currentState == WeaponStates.WeaponIdle)
        {
            WeaponTriggerOn();
        }
    }

    public virtual void WeaponTriggerOn()
    {
        //todo play sound
        //todo modify player movement speed
        weaponStates.ChangeState(WeaponStates.WeaponStart);

    }

    public virtual void WeaponTriggerOFF()
    {
        if(weaponStates.currentState == WeaponStates.WeaponIdle || weaponStates.currentState == WeaponStates.WeaponStop)
        {
            return;
        }

        weaponStates.ChangeState(WeaponStates.WeaponStop);
    }

    public virtual void FireRequest()
    {
        weaponStates.ChangeState(WeaponStates.WeaponUse);
    }

    public virtual void ProjectileWeaponFire()
    {
        //todo player VFX
        //todo play SFX
        //todo shootProjectile
        _timeBetweenFireCounter = timeBetweenFire;
        weaponStates.ChangeState(WeaponStates.WeaponDelayBetweenUses);
    }

    public IEnumerator MeleeWeaoponFire()
    {
        if(_isMeleeAttacking)
        {
           yield break;
        }
        _isMeleeAttacking = true;
        yield return new WaitForSeconds(initialDamageDelay);
        Debug.Log("TDWeapon : " + "EnableDamage Area");
        //todo create a weapon damage area;
        MeleeWeaponDamageAreaON();
        yield return new WaitForSeconds(activeDamageDuration);
        Debug.Log("TDWeapon : " + "Disable Damage Area");
        yield return new WaitForSeconds(afterDamageDelay);
        _isMeleeAttacking = false;
        Debug.Log("Weapon Delay Between Uses");
        _timeBetweenFireCounter = timeBetweenFire;
        weaponStates.ChangeState(WeaponStates.WeaponDelayBetweenUses);



    }
    //initialize weapon animator parameters
    protected virtual void InitializeAnimatorParameters()
    {
        TDAnimatorManager.AddAnimatorParamaterIfExists(weaponAnimator, IdleAnimationParameter, AnimatorControllerParameterType.Bool, weaponAnimatorParameters);
        TDAnimatorManager.AddAnimatorParamaterIfExists(weaponAnimator, StartAnimationParameter, AnimatorControllerParameterType.Bool, weaponAnimatorParameters);
        TDAnimatorManager.AddAnimatorParamaterIfExists(weaponAnimator, ChargingStartAnimationParameter, AnimatorControllerParameterType.Bool, weaponAnimatorParameters);
        TDAnimatorManager.AddAnimatorParamaterIfExists(weaponAnimator, DelayBeforeUseAnimationParameter, AnimatorControllerParameterType.Bool, weaponAnimatorParameters);
        TDAnimatorManager.AddAnimatorParamaterIfExists(weaponAnimator, UseAnimationParameter, AnimatorControllerParameterType.Bool, weaponAnimatorParameters);
        TDAnimatorManager.AddAnimatorParamaterIfExists(weaponAnimator, StopAnimationParameter, AnimatorControllerParameterType.Bool, weaponAnimatorParameters);
    }
    //update weapon animator , other than character's animator
    public virtual void UpdateAnimators()
    {
        TDAnimatorManager.UpdateBoolParamater(weaponAnimator, IdleAnimationParameter, (weaponStates.currentState == WeaponStates.WeaponIdle), weaponAnimatorParameters);
        TDAnimatorManager.UpdateBoolParamater(weaponAnimator, StartAnimationParameter, (weaponStates.currentState == WeaponStates.WeaponStart), weaponAnimatorParameters);
        TDAnimatorManager.UpdateBoolParamater(weaponAnimator, ChargingStartAnimationParameter, (weaponStates.currentState == WeaponStates.WeaponStart && isChargingCompleted), weaponAnimatorParameters);
        TDAnimatorManager.UpdateBoolParamater(weaponAnimator, DelayBeforeUseAnimationParameter, (weaponStates.currentState == WeaponStates.WeaponDelayBeforeUse), weaponAnimatorParameters);
        TDAnimatorManager.UpdateBoolParamater(weaponAnimator, UseAnimationParameter, (weaponStates.currentState == WeaponStates.WeaponUse), weaponAnimatorParameters);
        TDAnimatorManager.UpdateBoolParamater(weaponAnimator, StopAnimationParameter, (weaponStates.currentState == WeaponStates.WeaponStop), weaponAnimatorParameters);

    }

    public void WeaponON()
    {
        isUpdate = true;
    }
    public void WeaponOFF()
    {
        isUpdate = false;
    }
}

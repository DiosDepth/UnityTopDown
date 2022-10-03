using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponNameSpace;

namespace WeaponNameSpace
{
    public enum AimType
    {
        None,
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
        SR,
    }

    public enum AvalibleWeapons
    {
        None,
        Sword,
        ShotGun,
        SniperRifle,
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

    //make enum same as Projectiles in Resources\GamePrefab\Weapons\Projectiles\
    public enum AvalibleProjectile
    {
        PistolBullet,
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
    public string bulletPrefabPath = @"GamePrefab\Weapons\Projectiles\";


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

    public Transform firePoint;

    [Header("Melee Weapon Damage Settings")]
    public float meleeWeaponInitialDamageDelay =0.3f;
    public float meleeWeaponActiveDamageDuration = 0.3f;
    public float meleeWeaponAfterDamageDelay = 0.4f;

    public LayerMask meleeWeaponDamageMask = 1 << 9;
    public Vector3 meleeWeaponDamageAreaSize = new Vector3(1,1,1);
    public Vector3 meleeWeaponDamageAreaOffset = new Vector3(0,0,1);
    public bool showDamageArea = false;


    [Header("Projectile Weapon Damage Settings")]
    public WeaponShottingType shottingType = WeaponShottingType.SG;
    public AvalibleProjectile bullet = AvalibleProjectile.PistolBullet;
    
    public float projectileTotalDamage = 20;
    public float projectileScatterAngle = 60;
    public int projectileScatterCount = 3;
    protected List<Vector2> _projectilescatterinfo = new List<Vector2>();
    protected List<Vector2>.Enumerator _projectilescatterEnumerator;

    [Header("Instance Weapon Damage Settings")]
    public LayerMask instanceWeaponDamageMask = 1 << 9;
    public float instanceWeaponTraceDistance = 5f;
    public float instanceWeaponScatterAngle = 60;
    public int intinstanceWeaponScatterCount = 3;
    public ContactFilter2D instanceWeaponFilter;
    protected RaycastHit2D[] _hitInfoList;
    

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
    //todo change string to animation info
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
    protected Vector3 _aimDirection;

    protected bool _isMeleeAttacking = false;
    // Start is called before the first frame update
    private bool isUpdate = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdate)
        {
            ProcessWeaponState();
            state = weaponStates.currentState;

        }

    }

    private void LateUpdate()
    {

        if (isUpdate)
        {
            if(weaponAnimator != null)
            {
                UpdateAnimators();
            }
        }
    }

    public virtual void Initialization()
    {
        //base.Initialization();
        weaponAnimator = GetComponentInChildren<Animator>();
        _movement = owner.transform.GetComponent<TDCharacterAbilityMovement>();
        _weaponHandler = owner.transform.GetComponent<TDCharacterAbilityHandleWeapon>();

        if(firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
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
        damagedCollider = Physics.OverlapBox(_movement.rotationRoot.TransformPoint(meleeWeaponDamageAreaOffset), meleeWeaponDamageAreaSize, Quaternion.identity, owner.attribute.damageMask);
        
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
            Gizmos.DrawWireCube(_movement.rotationRoot.TransformPoint(meleeWeaponDamageAreaOffset), meleeWeaponDamageAreaSize);
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
    {
        switch (weaponType)
        {
            case WeaponType.Projectile:
                ProjectileWeaponFire();
                break;
            case WeaponType.Melee:
                StartCoroutine(MeleeWeaoponFire());
                break;
            case WeaponType.Instante:
                InstanteWeaponFire();
                break;
        }



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

        _timeBetweenFireCounter = timeBetweenFire;
        //todo player VFX
        //todo play SFX
        _aimDirection = (owner.GetAbilityByUniqueSkillName("HandleWeapon") as TDCharacterAbilityHandleWeapon).aimingDirection;

        
        _projectilescatterinfo = ExtensionMathTools.DivideAngleByCountXY(_aimDirection, projectileScatterAngle, projectileScatterCount);
        _projectilescatterEnumerator = _projectilescatterinfo.GetEnumerator();//create a Enumerator for spawning projectile with scatter angle.
        

        for (int i = 0; i < _projectilescatterinfo.Count; i++)
        {
            TDPoolManager.instance.GetObj(bulletPrefabPath + bullet.ToString(), true, (obj) =>
            {
                StraightLineProjectile prjc = obj.GetComponent<StraightLineProjectile>();
                prjc.ownerWeapon = this;

                //you can't just using _scatterinfo[i], cus projectile spawned in GetObj which is a Coroutine method, when the method called [i] already looped all the index of _scatterinfo and lager than _scatterinfo.Count
                // it cus _scatterinfo[i] out of range
                _projectilescatterEnumerator.MoveNext();
                prjc.flyingDirection = _projectilescatterEnumerator.Current;
                
                prjc.transform.position = firePoint.transform.position;
                prjc.transform.rotation = Quaternion.FromToRotation(Vector3.right, prjc.flyingDirection);
                prjc.Initialization();
            });

        }
        weaponStates.ChangeState(WeaponStates.WeaponDelayBetweenUses);
    }



    public virtual IEnumerator MeleeWeaoponFire()
    {
        if(!_isMeleeAttacking)
        {
           yield break;
        }
        _isMeleeAttacking = true;
        yield return new WaitForSeconds(meleeWeaponInitialDamageDelay);
        Debug.Log("TDWeapon : " + "EnableDamage Area");
        //todo create a weapon damage area;
        MeleeWeaponDamageAreaON();
        yield return new WaitForSeconds(meleeWeaponActiveDamageDuration);
        Debug.Log("TDWeapon : " + "Disable Damage Area");
        yield return new WaitForSeconds(meleeWeaponAfterDamageDelay);
        _isMeleeAttacking = false;
        Debug.Log("Weapon Delay Between Uses");
        _timeBetweenFireCounter = timeBetweenFire;
        weaponStates.ChangeState(WeaponStates.WeaponDelayBetweenUses);
    }



    public virtual void InstanteWeaponFire()
    {
        _timeBetweenFireCounter = timeBetweenFire;
        _aimDirection = (owner.GetAbilityByUniqueSkillName("HandleWeapon") as TDCharacterAbilityHandleWeapon).aimingDirection;


        List<Vector2> temp_scaterinfo = ExtensionMathTools.DivideAngleByCountXY(_aimDirection, instanceWeaponScatterAngle, intinstanceWeaponScatterCount);

       
        for (int i = 0; i < temp_scaterinfo.Count; i++)
        {

            _hitInfoList = Physics2D.LinecastAll(firePoint.transform.position, temp_scaterinfo[i] * instanceWeaponTraceDistance + firePoint.transform.position.ToVector2(), instanceWeaponDamageMask);
            Debug.DrawLine(firePoint.position, temp_scaterinfo[i] * instanceWeaponTraceDistance + firePoint.transform.position.ToVector2(), Color.red, 0.5f);

        }




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


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class Projectile : TDPoolableGameObject
{
    [SerializeField]
    public TDWeapon ownerWeapon;
    [SerializeField]
    public Vector2 flyingDirection
    {
        get { return _flyingDirection; }
        set { _flyingDirection = value; }
    }
    public bool isUpdateRotation;
    [SerializeField]
    public float flyingSpeed;
    [SerializeField]
    public float lifeTime = 10;
    [SerializeField]
    public LayerMask hitMask;
    [SerializeField]
    public ParticleSystem hitVFX;


    protected RaycastHit2D _hitinfo;

    protected Vector2 _flyingDirection;
    [SerializeField]
    public ContactFilter2D _filter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdate)
        {
            ProjectileUpdate();
        }
    }

    public virtual void ProjectileUpdate()
    {

    }

    IEnumerator Live()
    {

        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }

    private void PlayVFX()
    {
        
    }

    protected virtual void ApplyDamage()
    {
        
    }


    public override void Initialization()
    {

        SetUpdate(true);
        if(lifeTime > 0)
        {
            StartCoroutine("Live");
        }
    }

    public override void Destroy()
    {
        StopCoroutine("Live");
        base.Destroy();
    }




}

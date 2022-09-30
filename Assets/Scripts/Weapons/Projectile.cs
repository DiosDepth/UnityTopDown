using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : TDPoolableGameObject
{
    public TDWeapon ownerWeapon;
    public Vector2 flyingDirection;
    public float flyingSpeed;
    public float lifeTime = 10;
    public LayerMask hitMask;

    public ParticleSystem hitVFX;


    private RaycastHit2D _hitinfo;
    private Vector3 lastFramPos;
    public ContactFilter2D _filter;
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isUpdate)
        {
            return;
        }
        transform.Translate(flyingDirection * flyingSpeed * Time.deltaTime);
        _hitinfo = Physics2D.CircleCast(transform.position, transform.lossyScale.x / 2, transform.forward, GetLastFrameDistance(transform.position), hitMask);
        if (_hitinfo)
        {
            ExtDebug.DrawBox(lastFramPos, transform.lossyScale, transform.rotation, Color.red);
            ExtDebug.DrawBox(transform.position, transform.lossyScale, transform.rotation, Color.red);

            //PlayVFX();
            ApplyDamage();
            SetUpdate(false);
            //Destroy();
           
        }
   

        lastFramPos = transform.position;
    }

    IEnumerator Live()
    {

        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }

    private void PlayVFX()
    {
        
    }

    private void ApplyDamage()
    {
        
    }

    public float GetLastFrameDistance(Vector3 thisFramPos)
    {
        float distance;
        distance = Vector3.Distance(thisFramPos, lastFramPos);

        return distance;
    }

    public override void Initialization()
    {
        lastFramPos = transform.position;
        if (flyingDirection == Vector2.zero)
        {
            flyingDirection = Vector2.up;
        }
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

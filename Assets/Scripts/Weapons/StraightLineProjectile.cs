using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineProjectile : Projectile
{

    private Vector3 lastFramPos;

    public override void ProjectileUpdate()
    {
        base.ProjectileUpdate();
        transform.Translate(transform.InverseTransformDirection(_flyingDirection) * flyingSpeed * Time.deltaTime);
        
        _hitinfo = Physics2D.CircleCast(lastFramPos, transform.lossyScale.x / 2, _flyingDirection, Vector2.Distance(transform.position, lastFramPos), hitMask);
        if (_hitinfo)
        {

            //PlayVFX();
            ApplyDamage();

            Destroy();

        }
        lastFramPos = transform.position;
    }

    public override void Initialization()
    {
        base.Initialization();
        lastFramPos = transform.position;
        if (_flyingDirection == Vector2.zero)
        {
            _flyingDirection = Vector2.right;
        }
    }
}

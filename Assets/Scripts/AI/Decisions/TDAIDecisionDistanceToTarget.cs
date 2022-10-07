using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace TDEnums
{
    public enum CompareType
    {
        Greater,
        GreaterEqula,
        Equla,
        LessEqula,
        Less,
        NonEqula,
    }
}

public class TDAIDecisionDistanceToTarget : TDAIDecision
{
    public bool isDebug = false;
    public Transform target;
    public TDEnums.CompareType compare = TDEnums.CompareType.GreaterEqula;
    public float thresholdDistance;

    /// <summary>
    /// masklay that block AI to see sth
    /// </summary>

    public override void Initialization()
    {
        base.Initialization();
        
    }

    public override void OnEnterDecision()
    {
        base.OnEnterDecision();

        

    }

    public override bool Decide()
    {
       return DistanceToTarget();
        
    }



    public override void OnExitDecision()
    {
        base.OnExitDecision();
    }

    public bool DistanceToTarget()
    {
        if(target == null)
        {
            GetTarget();
            if (target == null)
                return false;
        }
        bool result = false;
        switch (compare)
        {
            case TDEnums.CompareType.Greater:
                if (transform.position.SqrDistanceXY(target.position) > Mathf.Pow(thresholdDistance, 2))
                {
                    result = true;
                }
                break;
            case TDEnums.CompareType.GreaterEqula:
                if (transform.position.SqrDistanceXY(target.position) >= Mathf.Pow(thresholdDistance, 2))
                {
                    result = true;
                }
                break;
            case TDEnums.CompareType.Equla:
                if (transform.position.EqualXY(target.position))
                {
                    result = true;
                }
                break;
            case TDEnums.CompareType.LessEqula:
                if (transform.position.SqrDistanceXY(target.position) <= Mathf.Pow(thresholdDistance, 2))
                {
                    result = true;
                }
                break;
            case TDEnums.CompareType.Less:
                if (transform.position.SqrDistanceXY(target.position) < Mathf.Pow(thresholdDistance, 2))
                {
                    result = true;
                }
                break;
            case TDEnums.CompareType.NonEqula:
                if (transform.position.SqrDistanceXY(target.position) != Mathf.Pow(thresholdDistance, 2))
                {
                    result = true;
                }
                break;
        }

        return result;
    }

    public void OnDrawGizmos()
    {
        if(!isDebug)
        {
            return;
        }
       
    }

    public void GetTarget()
    {
        if (_brain.GetHearingPlayerTarget() != null)
        {
            target = _brain.GetHearingPlayerTarget().transform;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TDAIActionFollowTarget : TDAIActionMove
{

    public Transform currentTarget;
    public Transform previousTarget;
    public float followFrequency = 0.5f;


    protected float selfRadius;
    protected float targetRadius;

    protected float _nextUpdateTime;



    public bool followWhenReached;

    public override void Initialization()
    {
        base.Initialization();
    }
    public override void OnEnterAction()
    {
        
        selfRadius = GetComponent<CapsuleCollider2D>().size.x;
        UpdateTarget(GetTarget().transform);
        CalculatePath();
    }
    public override void UpdateAction()
    {
        if (Time.time > _nextUpdateTime)
        {
            if(GetTarget() == null)
            {
                return;
            }
            UpdateTarget(GetTarget().transform);
            if (IsTargetPosChange())
            {
                _moveStart = false;
                isReached = false;
                CalculatePath();
            }
            StartMove();
            _nextUpdateTime += followFrequency;
        }

    }
    public override void OnExitAction()
    {
        base.OnExitAction();
    }

    protected override void StartMove()
    {
        if (_moveStart)
        {
            if (transform.position.EqualXY(_nextPos))
            {
                _pathIndex++;
                if (_pathIndex >= path.corners.Length)
                {
                    _dir = Vector3.zero;
                    _movement.SetMoveDirection(_dir);
                    isReached = true;
                    _moveStart = false;
                    return;
                }
                else
                {
                    _nextPos = path.corners[_pathIndex];//获取路径上的下一个位置点，用来计算方向或者其他
                    if (_pathIndex == path.corners.Length - 1)
                    {
                        Vector3 p1 = path.corners[_pathIndex - 1];
                        Vector3 p2 = path.corners[_pathIndex];
                        float tolerentdst = CalculateTargetTolerentDistance(this.transform, currentTarget);
                        _nextPos = (p1 - p2).normalized * tolerentdst + _nextPos;//计算最终点，这个点包括了位置容差。
                    }

  
                }
            }
            if (transform.position.SqrDistanceXY(currentTarget.position) <= _nextPos.SqrDistanceXY(currentTarget.position))
            {
                _dir = Vector3.zero;
                _movement.SetMoveDirection(_dir);
                isReached = true;
                _moveStart = false;
                return;
            }
            _dir = transform.position.DirectionToXY(_nextPos);
            _movement.SetMoveDirection(_dir);
        }
    }


    protected float CalculateTargetTolerentDistance(Transform selfTRS, Transform targetTRS)
    {
        RaycastHit2D hit;//通过打一条射线，获取射线击中的点，从这个点到物体中心距离作为TargetRadius


        hit = Physics2D.Raycast(selfTRS.position, (targetTRS.position - selfTRS.position).normalized, selfTRS.position.DistanceXY(targetTRS.position), 1 << targetTRS.gameObject.layer);
        if (hit)
        {
            targetRadius = Vector2.Distance(hit.point,targetTRS.position);
        }
        else
        {
            targetRadius = 0.5f;
        }

        return Mathf.Min( transform.position.DistanceXY(currentTarget.position), (selfRadius + targetRadius + stoppingDistance));//区一个较小的值作为最终点的位置容差
    }

    private bool UpdateTarget(Transform newtarget)
    {
        if(currentTarget.Equals(newtarget))
        {
            return false;
        }

        previousTarget = currentTarget;
        currentTarget = newtarget;
        //targetRadius = currentTarget.GetComponent<CharacterController>().radius;
        _moveStart = false;
        isReached = false;
        return true;
    }

    protected override Vector3 GetTargetPos()
    {
        return currentTarget.transform.position;
    }

    protected bool IsTargetPosChange()
    {
        if(targetPos.EqualXY(currentTarget.transform.position))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (i ==0)
            {
                Gizmos.DrawLine(transform.position, path.corners[i]);
            }
            else
            {
                Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);
            }
            if(i == path.corners.Length -1)
            {
                Gizmos.DrawLine(path.corners[i], targetPos);
            }
      
               
            
           
            Gizmos.DrawWireSphere(path.corners[i], 0.1f);
        }
    }
}

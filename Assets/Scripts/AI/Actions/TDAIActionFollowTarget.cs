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
        selfRadius = GetComponent<CharacterController>().radius;
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
            if (transform.position.HorizontalEqula(_nextPos))
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
            if (transform.position.HorizontalSqrDistance(currentTarget.position) <= _nextPos.HorizontalSqrDistance(currentTarget.position))
            {
                _dir = Vector3.zero;
                _movement.SetMoveDirection(_dir);
                isReached = true;
                _moveStart = false;
                return;
            }
            _dir = transform.position.HorizontalDirctionTo(_nextPos);
            _movement.SetMoveDirection(_dir);
        }
    }


    protected float CalculateTargetTolerentDistance(Transform selfTRS, Transform targetTRS)
    {
        RaycastHit hit;//通过打一条射线，获取射线击中的点，从这个点到物体中心距离作为TargetRadius
  
        if (Physics.Raycast(selfTRS.position, (targetTRS.position - selfTRS.position).normalized, out hit, targetTRS.position.HorizontalSqrDistance(selfTRS.position), 1 << targetTRS.gameObject.layer))
        {
            targetRadius = hit.point.HorizontalDistance(targetTRS.position);
        }
        else
        {
            targetRadius = 0.5f;
        }

        return Mathf.Min(transform.position.HorizontalDistance(currentTarget.position), (selfRadius + targetRadius + stoppingDistance));//区一个较小的值作为最终点的位置容差
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
        if(targetPos.HorizontalEqula(currentTarget.transform.position))
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
        if(_nextPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_nextPos, 0.3f);
        }
    }
}

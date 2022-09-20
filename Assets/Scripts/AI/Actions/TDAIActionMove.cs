using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TDAIActionMove : TDAIAction
{

    public Vector3 targetPos;
    public bool isReached;
    public float stoppingDistance = 0.5f;
    public float changeThreshold = 0.5f;




    protected NavMeshPath path;
    protected TDCharacterAbilityMovement _movement;
    protected Vector3 targetPreviousPos;
    protected IEnumerator _waitForCalculatePath;
    protected bool _moveStart = false;
    protected Vector3 _dir = Vector3.zero;
    protected int _pathIndex = 0;
    protected Vector3 _nextPos;








    public override void Initialization()
    {
        base.Initialization();

        _movement = GetComponent<TDCharacterAbilityMovement>();
        path = new NavMeshPath();
    }

    public override void OnEnterAction()
    {
        base.OnEnterAction();
       
        CalculatePath();

    }

    public override void UpdateAction()
    {
        StartMove();
    }

    public override void OnExitAction()
    {
        base.OnExitAction();
        StopCoroutine("WaitForPathCalculate");
        _dir = Vector3.zero;
        _movement.SetMoveDirection(_dir);
    }

    protected virtual void CalculatePath()
    {
        StopCoroutine("WaitForPathCalculate");
        StartCoroutine("WaitForPathCalculate");
    }

    protected virtual IEnumerator WaitForPathCalculate()
    {
        
        targetPos = GetTargetPos();
        _brain.agent.CalculatePath(targetPos, path);
        while (true)
        {
            if (_brain.agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                OnPathComplete(path);
                break;
            }
            if (_brain.agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                
                break;
            }
            yield return null;
        }
    }

    protected virtual void OnPathComplete(NavMeshPath path)
    {
        _pathIndex = 0;
        _moveStart = true;
        isReached = false;
        _dir = Vector3.zero;
        _nextPos = path.corners[0];
    }

    protected virtual void StartMove()
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
                        float tolerentdst = CalculateTargetTolerentDistance();
                        _nextPos = (p1 - p2).normalized * tolerentdst + _nextPos;//计算最终点，这个点包括了位置容差。
                    }
                }
            }
            _dir = transform.position.HorizontalDirctionTo(_nextPos);
            _movement.SetMoveDirection(_dir);
        }
    }


    protected float CalculateTargetTolerentDistance()
    {
        return stoppingDistance;
    }

    protected virtual GameObject GetTarget()
    {
        return _brain.GetHearingPlayerTarget();
    }

    protected virtual Vector3 GetTargetPos()
    {
        return GetTarget().transform.position;
    }

}

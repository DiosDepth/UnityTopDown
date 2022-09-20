using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDAIActionSee : TDAIAction
{

    public bool isDebug = false;

    public float seeingFrequency = 0.5f;
    public float seeingRadius = 10f;
    public float seeingAngle = 110;
    public LayerMask seeingMask;
    public LayerMask seeingBlockMask;

    private List<int> _seeingMaskValue;
    private Vector3 _faceDirection;
    private TDCharacterAbilityMovement _movement;
    private float _nextUpdateTime;

    public override void Initialization()
    {
        base.Initialization();
        _movement = GetComponent<TDCharacterAbilityMovement>();
        _seeingMaskValue = GetMaskValue(seeingMask);
    }

    public override void OnEnterAction()
    {
        base.OnEnterAction();
    }

    public override void UpdateAction()
    {
        if (Time.time > _nextUpdateTime)
        {
            Seeing();
            _nextUpdateTime += seeingFrequency;
        } 

    }

    public override void OnExitAction()
    {
        base.OnExitAction();
        //_brain.seeingDic.Clear();
    }

    public void Seeing()
    {
        string layername;
        Collider[] temp = Physics.OverlapSphere(transform.position, seeingRadius, seeingMask); // get collider in raduis
        _brain.seeingDic.Clear();
        RaycastHit hit;
        _faceDirection = _movement.faceDirection;

        for (int i = 0; i < temp.Length; i++)
        {
            layername = LayerMask.LayerToName(temp[i].gameObject.layer);
            if (Vector3.Angle(_faceDirection, (temp[i].transform.position - transform.position).normalized) <= seeingAngle / 2) // check angle
            {
                if (Physics.Raycast(transform.position, transform.position.HorizontalDirctionTo(temp[i].gameObject.transform.position),out hit,seeingRadius,seeingMask)) //check blocker
                {
                    if(hit.transform.gameObject.Equals(temp[i].gameObject))
                    {

                        if(!_brain.seeingDic.ContainsKey(layername))
                        {
                            _brain.seeingDic.Add(layername, new List<GameObject>() { temp[i].gameObject });
                        }
                        else
                        {
                            _brain.seeingDic[layername].Add(temp[i].gameObject);
                        }

                    }
                }
                
            }
        }
    }

    private List<int> GetMaskValue(LayerMask mask)
    {
        List<int> tempvalue = new List<int>();
        for (int i = 0; i < 32; i++)
        {
            if ((mask.value & (1 << i)) != 0)
            {
                Debug.Log("SeeLayerMaskValue : " + i);
                tempvalue.Add(i);
            }
        }
        return tempvalue;
    }

    private void OnDrawGizmos()
    {
        if (!isDebug)
        {
            return;
        }

        if (_brain == null)
        {
            return;
        }
        if (_brain.seeingDic != null && _brain.seeingDic.Count > 0)
        {
            Gizmos.color = Color.green;
            foreach (KeyValuePair<string, List<GameObject>> see in _brain.seeingDic)
            {
                for (int i = 0; i < see.Value.Count; i++)
                {
                    Gizmos.DrawLine(transform.position, see.Value[i].transform.position);
                }
            }
          
        }
    }
}

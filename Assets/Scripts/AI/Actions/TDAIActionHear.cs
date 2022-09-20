using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDAIActionHear : TDAIAction
{
    public bool isDebug = false;

    public float hearingFrequency = 0.5f;
    public float hearingRadius = 10f;
    public LayerMask hearingMask;


    private List<int> _hearingMaskValue;
    private float _nextUpdateTime;

    public override void Initialization()
    {
        base.Initialization();
        _hearingMaskValue = GetMaskValue(hearingMask);
    }

    public override void OnEnterAction()
    {
        base.OnEnterAction();
    }

    public override void UpdateAction()
    {
        if (Time.time > _nextUpdateTime)
        {
            Hearing();
            _nextUpdateTime += hearingFrequency;
        }
    
    }

    public override void OnExitAction()
    {
        base.OnExitAction();
       // _brain.hearingDic.Clear();
    }

    public void Hearing()
    {
        string layername;
        Collider[] temp = Physics.OverlapSphere(transform.position, hearingRadius, hearingMask);
        _brain.hearingDic.Clear();

        for (int i = 0; i < temp.Length; i++)
        {
            if(_hearingMaskValue.Contains(temp[i].gameObject.layer))
            {
                Debug.Log("Hear object layer : " + temp[i].gameObject.layer);
                layername = LayerMask.LayerToName(temp[i].gameObject.layer);
                if (!_brain.hearingDic.ContainsKey(layername))
                {
                    _brain.hearingDic.Add(layername, new List<GameObject>() { temp[i].gameObject });
                }
                else
                {
                    _brain.hearingDic[layername].Add(temp[i].gameObject);
                }
            }
        }
    }

    private List<int> GetMaskValue(LayerMask mask)
    {
        List<int> tempvalue = new List<int>();
        for (int i = 0; i < 32; i++)
        {
            if((mask.value & (1<<i)) != 0)
            {
                Debug.Log("HearLayerMaskValue : " + i);
                tempvalue.Add(i);
            }
        }
        
        return tempvalue;
    }

    private void OnDrawGizmos()
    {
        if(!isDebug)
        {
            return;
        }
        if(_brain == null)
        {
            return;
        }
        if (_brain.hearingDic != null &&_brain.hearingDic.Count >0)
        {
            Gizmos.color = Color.red;
            foreach(KeyValuePair<string,List<GameObject>> hear in _brain.hearingDic)
            {
                for (int i = 0; i < hear.Value.Count; i++)
                {
                    Gizmos.DrawSphere(hear.Value[i].transform.position, 0.5f);
                }
            }
           
        }
    }
}

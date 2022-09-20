using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;
using UnityEngine.Events;

public struct ObjPool
{
    public GameObject fatherObj;
    public List<GameObject> objList;

    public ObjPool(GameObject m_obj, GameObject m_poolholder)
    {
        fatherObj = new GameObject(m_obj.name);
        fatherObj.transform.parent = m_poolholder.transform;
        fatherObj.SetActive(false);
        m_obj.transform.SetParent(fatherObj.transform);
        objList = new List<GameObject>() {m_obj};
    }

    public void PushObj(GameObject m_obj)
    {
        m_obj.SetActive(false);
        m_obj.transform.parent = fatherObj.transform;
        objList.Add(m_obj);
    }
    public GameObject GetObj(bool m_isactive)
    {
        GameObject temp_obj = objList[0];
        objList.RemoveAt(0);
        temp_obj.SetActive(m_isactive);
        temp_obj.transform.parent = null;
        
        return temp_obj;

    }
}

public class TDPoolManager : Singleton<TDPoolManager>
{
    public bool isTestMode;
    public Dictionary<string, ObjPool> poolDic = new Dictionary<string, ObjPool>();
    public GameObject poolObj;

    public override void Start()
    {
        base.Start();
        if(isTestMode)
        {
            Initialization();
        }
    }

    public override void Initialization()
    {
        if(poolObj == null)
        {
            poolObj = new GameObject("PoolHolder");
        }

        TDManagerEvent.Trigger(ManagerEventType.InitialCompleted, this.gameObject.name);
        Debug.Log("TDPoolManager : " + instance.gameObject.name);
    }


    public void PushObj(string m_key, GameObject m_obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("PoolHolder");
        if(poolDic.ContainsKey(m_key))
        {
            poolDic[m_key].PushObj(m_obj);
        }
        else
        {
            poolDic.Add(m_key, new ObjPool(m_obj, poolObj));
        }
    }

    public void GetObj(string m_key,bool m_isactive, UnityAction<GameObject> callback)
    {
        
        if(poolDic.ContainsKey(m_key) && poolDic[m_key].objList.Count > 0)
        {
            callback?.Invoke(poolDic[m_key].GetObj(m_isactive));
        }
        else
        {
            TDDataManager.instance.LoadResAsync<GameObject>(m_key,(obj) => {
                obj.name = m_key;
                obj.SetActive(m_isactive);
                callback?.Invoke(obj);
            });
            
        }

    }

    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }



}

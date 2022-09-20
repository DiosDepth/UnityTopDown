using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;

public abstract class DataInfo
{
    public int ID;
    public string Name;
    public DataInfo() { }
    public abstract void Initial(string[] info);

}

public class AIDataInfo : DataInfo
{
    
    public AIType Type;

    public string PrefabPath;
    public AIDataInfo() { }
    public AIDataInfo(int m_id, string m_name, AIType m_type, string m_prefabpath)
    {
        ID = m_id;
        Type = m_type;
        Name = m_name;
        PrefabPath = m_prefabpath;

    }
    public AIDataInfo(string m_id, string m_name, string m_type, string m_prefabpath)
    {
        int.TryParse(m_id, out ID);
        Type = (AIType)AIType.Parse(typeof(AIType), m_type);
        Name = m_name;
        PrefabPath = m_prefabpath;
    }

    public AIDataInfo(string[] info)
    {
        int.TryParse(info[0], out ID);
        Name = info[1];
        Type = (AIType)AIType.Parse(typeof(AIType), info[2]);
        PrefabPath = info[3];
    }

    public override void Initial(string[] info)
    {
        int.TryParse(info[0], out ID);
        Name = info[1];
        Type = (AIType)AIType.Parse(typeof(AIType), info[2]);
        PrefabPath = info[3];
    }
}

public class WeaponDataInfo : DataInfo
{
    public string WeaponType;
    public string PrefabPath;

    public WeaponDataInfo() { }
    public WeaponDataInfo(string[] info)
    {
        int.TryParse(info[0], out ID);
        Name = info[1];
        WeaponType = info[2];
        PrefabPath = info[3];
    }

    public override void Initial(string[] info)
    {
        int.TryParse(info[0], out ID);
        Name = info[1];
        WeaponType = info[2];
        PrefabPath = info[3];
    }
}

public class TDGameData 
{

}

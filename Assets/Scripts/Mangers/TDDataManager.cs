using System.Collections;
using System.Collections.Generic;
using System.IO;
using TDEnums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace TDEnums
{
    public enum TDDataManagerEventType
    {
        DataLoadingRequest,
        DataLoading,
        DataLoaded,
    }
}


[System.Serializable]
public struct GameDataABInfo
{
    public string dataName;
    public string ABPath;
}

public struct TDDataManagerEvent
{
    public TDDataManagerEventType evtType;
    public float progress;
    public TDDataManagerEvent(TDDataManagerEventType m_type, float m_progress)
    {
        evtType = m_type;
        progress = m_progress;
    }

    static TDDataManagerEvent e;
    public static void Trigger(TDDataManagerEventType m_type,float m_progress)
    {
        e.evtType = m_type;
        e.progress = m_progress;
        TDEventManager.TriggerEvent<TDDataManagerEvent>(e);
    }
}


public struct FloatChange
{
    public string varName;
    public float value;
    public FloatChange(string m_varname, float m_value)
    {
        varName = m_varname;
        value = m_value;
    }
    static FloatChange e;
    public static void Trigger(string m_varname, float m_value)
    {
        e.varName = m_varname;
        e.value = m_value;
        TDEventManager.TriggerEvent<FloatChange>(e);
    }
}

public struct IntChange
{
    string varName;
    float value;
    public IntChange(string m_varname, float m_value)
    {
        varName = m_varname;
        value = m_value;
    }
    static IntChange e;
    public static void Trigger(string m_varname, float m_value)
    {
        e.varName = m_varname;
        e.value = m_value;
        TDEventManager.TriggerEvent<IntChange>(e);
    }
}


public class TDDataManager : Singleton<TDDataManager>
{ 
    public bool isTestMode; 
    public FileInfo[] csvInfo;
    public string resourcePath;

    public long totalLength;
    private long curProgress;
    public long CurrentProgress { get { return curProgress; } }
    [SerializeField]
    public GameDataABInfo AIDataInfo;

    [SerializeField]
    public GameDataABInfo WeaponDataInfo;


    //public Dictionary<CSVDataType, Dictionary<string, Data>> csvDic = new Dictionary<CSVDataType, Dictionary<string, Data>>();
    private Dictionary<string, WeaponDataInfo> WeaponDataDic = new Dictionary<string, WeaponDataInfo>();
    private Dictionary<string, AIDataInfo> AIDataDic = new Dictionary<string, AIDataInfo>();

    private string _relativePath;
    //private AssetBundle ABData;
    
    
    public override void Awake()
    {
        base.Awake();
        
        Initialization();
    }

    public override void Start()
    {
        if(isTestMode)
        {
            Initialization();
            CollecteCSVDataInfo();
        }
    }

    public override void Initialization()
    {
        base.Initialization();
        Debug.Log("TDDataManager : " + instance.gameObject.name);
        _relativePath = System.IO.Directory.GetCurrentDirectory();
        TDManagerEvent.Trigger(ManagerEventType.InitialCompleted, this.gameObject.name);
    }



        public string GetCSVDataPath(string m_csvname)
    {
        string name = m_csvname.Substring(0, m_csvname.Length - 4);
        name = resourcePath + name;
        return name;
    }

    public float GetProgressRatio()
    {
        return (float)CurrentProgress/totalLength;
    }


    public void CollecteCSVDataInfo(UnityAction callback = null)
    {
        string relativepath = _relativePath + @"\Assets\Resources\Data";
        DirectoryInfo directoryInfo = new DirectoryInfo(relativepath);
        csvInfo = directoryInfo.GetFiles("*.csv");
        //Debug.Log("LongLength : " + csvInfo.LongLength);
        for (int i = 0; i < csvInfo.Length; i++)
        {
            totalLength += csvInfo[i].Length;
        }
        TDDataManagerEvent.Trigger(TDDataManagerEventType.DataLoadingRequest, GetProgressRatio());

        Debug.Log("TotalLength : " + totalLength);

        for (int i = 0; i < csvInfo.Length; i++)
        {
            LoadOnName(csvInfo[i]);
        }

        StartCoroutine(WaitForDataLoaded(callback));
    }

    IEnumerator WaitForDataLoaded(UnityAction callback)
    {
        while(GetProgressRatio() < 1)
        {
            TDDataManagerEvent.Trigger(TDDataManagerEventType.DataLoading, GetProgressRatio());
            yield return null;
        }

        TDDataManagerEvent.Trigger(TDDataManagerEventType.DataLoaded, GetProgressRatio());
        float waittingtime = Time.time + 3;
        bool iscontinue = false;
        while (!iscontinue)
        {
            if (Time.time >= waittingtime)
            {
                iscontinue = true;
            }
            yield return null;
        }
        yield return null;
        if (callback != null)
        {
            callback.Invoke();
        }
        yield return null;
    }


    public void LoadOnName(FileInfo m_fileinfo)
    {
        switch(m_fileinfo.Name)
        {
            case "AIData.csv":
                //StartCoroutine(LoadAIData(m_fileinfo, AIDataDic));
                StartCoroutine(LoadData<AIDataInfo>(m_fileinfo, AIDataDic));
                break;
            case "WeaponData.csv":
                //StartCoroutine(LoadWeaponData(m_fileinfo, WeaponDataDic));
                StartCoroutine(LoadData<WeaponDataInfo>(m_fileinfo, WeaponDataDic));
                break;
        }
    }

    public IEnumerator LoadData<T> (FileInfo m_info, Dictionary<string ,T> m_dic) where T: DataInfo,new()
    {
        string path = GetCSVDataPath(m_info.Name);
        LoadResAsync<TextAsset>(path, (t) =>
        {
            string[] row = t.text.Split(new char[] { '\r' });
            for (int i = 1; i < row.Length - 1; i++)
            {
                string[] column = row[i].Split(new char[] { ',' });
                T temp_data = new T();
                temp_data.Initial(column);
                m_dic.Add(temp_data.Name, temp_data);
                Debug.Log(typeof(T)+ row[i]);
            }
            curProgress += m_info.Length;
            //Debug.Log("CurLength : " + curProgress + " / TotalLength : " + totalLength + "--" + "AIDataProgress : " + (float)curProgress / totalLength * 100 + "%");
            //FloatChange.Trigger("GameLoadingProgress", GetProgressRatio());
        });
        yield return null;
    }

    public WeaponDataInfo GetWeaponDataInfo (AvalibleWeapons weapon)
    {
        string weaponname = weapon.ToString();
        if(WeaponDataDic.ContainsKey(weaponname))
        {
            return WeaponDataDic[weaponname];
        }
        else
        {
            return null;
        }
    }

    public AIDataInfo GetAIDataInfo(AvaliableAI ai)
    {
        string ainame = ai.ToString();
        if (AIDataDic.ContainsKey(ainame))
        {
            return AIDataDic[ainame];
        }
        else
        {
            return null;
        }
    }
   /* IEnumerator LoadAIData(FileInfo m_info, Dictionary<string,AIDataInfo> m_dic)
    {

        string path = GetCSVDataPath(m_info.Name);
        LoadAsync<TextAsset>(path, (t) => 
        {
            string[] row = t.text.Split(new char[] { '\r' });
            for (int i = 1; i < row.Length - 1; i++)
            {
                string[] column = row[i].Split(new char[] { ',' });
                AIDataInfo temp_data = new AIDataInfo(column);
                m_dic.Add(temp_data.Name, temp_data);
                Debug.Log("AIData :" + row[i]);
            }
            curProgress += m_info.Length;
            Debug.Log("CurLength : " + curProgress + " / TotalLength : " + totalLength + "--" + "AIDataProgress : " + (float)curProgress / totalLength * 100 + "%");
            FloatChange.Trigger("GameLoadingProgress", GetProgressRatio());
        });
        yield return null;


    }*/


    /*IEnumerator LoadWeaponData(FileInfo m_info, Dictionary<string, WeaponDataInfo> m_dic)
    {

        string path = GetCSVDataPath(m_info.Name);
        LoadAsync<TextAsset>(path, (t) => 
        {
            string[] row = t.text.Split(new char[] { '\r' });
            for (int i = 1; i < row.Length - 1; i++)
            {
                string[] column = row[i].Split(new char[] { ',' });
                WeaponDataInfo temp_data = new WeaponDataInfo(column);
                m_dic.Add(temp_data.Name, temp_data);
                Debug.Log("WeaponData :" + row[i]);

            }
            curProgress += m_info.Length;
            Debug.Log("CurLength : " + curProgress + " / TotalLength : " + totalLength + "--" + "WeaponDataProgress : " + (float)curProgress / totalLength * 100 + "%");
            FloatChange.Trigger("GameLoadingProgress", GetProgressRatio());
        });
        yield return null;


    }*/
 
    public T LoadRes<T>(string m_resrelevantpath) where T : Object
    {
        T res = Resources.Load<T>(m_resrelevantpath);
        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else
        {
            return res;
        }
    }

    public void LoadResAsync<T>(string m_resrelevantpath, UnityAction<T> callback) where T : Object
    {
       StartCoroutine(RealLoadResAsync<T>(m_resrelevantpath, callback));
    }


    private IEnumerator RealLoadResAsync<T>(string m_resrelevantpath, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(m_resrelevantpath);
        yield return r;// 如果上一步没有加载完毕，则这里的r== null，相当于 yield return null
        if(r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }

    
}
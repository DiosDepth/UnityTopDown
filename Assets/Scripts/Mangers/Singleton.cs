using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour,new()
{
    public static bool startUpdate;
    public static T instance
    {
        get
        {
            return _instance;
        }
    }
    protected static T _instance;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        if(_instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            _instance = this as T;
        }
        DontDestroyOnLoad(_instance);
    }
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Initialization()
    {

    }

    public static bool InstanceExsist()
    {
        bool exsist = false;
        if(_instance ==null)
        {
            Debug.Log("Instance of " + typeof(T) + " has no exsist");
            exsist = false;
        }
        else
        {
            exsist = true;
        }
        return exsist;
    }
}

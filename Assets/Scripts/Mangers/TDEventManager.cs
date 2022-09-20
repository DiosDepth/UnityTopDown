using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;
using System;
using UnityEngine.Events;

public static class TDEventManager 
{
    private static Dictionary<Type, List<BaseEventListener>> _subscriberList;
    static TDEventManager()
    {
        _subscriberList = new Dictionary<Type, List<BaseEventListener>>();
    }

    public static   void AddListener<T>(EventListener<T> listener) where T : struct
    {
        Type t = typeof(T);
        if(!_subscriberList.ContainsKey(t))
        {
            _subscriberList[t] = new List<BaseEventListener>();
        }
        if(!ListenerExists(t, listener))
        {
            _subscriberList[t].Add(listener);
        }
    }

    public static  void RemoveListener<T>(EventListener<T> listener) where T: struct
    {
        Type t = typeof(T);
       
        if(!_subscriberList.ContainsKey(t))
        {
            return;
        }
        List<BaseEventListener> list = _subscriberList[t];
        
        if(list.Count == 0)
        {
            _subscriberList.Remove(t);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == listener)
                {
                    list.Remove(list[i]);
                    return;
                }
            }
        }

    }

    public static  void TriggerEvent<T>(T evt) where T:struct
    {
        Type t = typeof(T);
        List<BaseEventListener> list;
        if (!_subscriberList.TryGetValue(t,out list))
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as EventListener<T>).OnEvent(evt);
        }
           
    }

    private static bool ListenerExists(Type type, BaseEventListener listener)
    {
        List<BaseEventListener> list;
        if(!_subscriberList.TryGetValue(type,out list))
        {
            return false;
        }
        bool exits = false;
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i] == listener)
            {
                exits = true;
                break;
            }
        }

        return exits;
    }
}

public static class TDEventRegister
{
    public static void StartListening<T>(this EventListener<T> caller) where T: struct
    {
        TDEventManager.AddListener<T>(caller);
    }
    public static void StopListening<T>(this EventListener<T> caller) where T: struct
    {
        TDEventManager.RemoveListener<T>(caller);
    }
}

public interface BaseEventListener
{

}
public interface EventListener <T>: BaseEventListener
{
    void OnEvent(T eventType);
}

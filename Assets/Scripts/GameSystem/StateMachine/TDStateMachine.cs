using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


public struct TDStateChangeEvent<T> where T:struct,IComparable,IConvertible,IFormattable
{
    public TDStateMachine<T> targetStateMachine;
    public T newState;
    public T previousState;
    public TDStateChangeEvent(TDStateMachine<T> statemachine)
    {
        targetStateMachine = statemachine;
        newState = statemachine.currentState;
        previousState = statemachine.previousState;
    }
}

public class TDStateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
{

    public GameObject target;
    public T currentState;
    public T previousState;
    public bool triggerEvent = false;

    public TDStateMachine(GameObject m_target, bool istrigger_event = false)
    {
        target = m_target;
        triggerEvent = istrigger_event;
    }

    public virtual void ChangeState( T newState)
    {
        if(newState.Equals(currentState))
        {
            return;
        }
        previousState = currentState;
        currentState = newState;
        if(triggerEvent)
            TDEventManager.TriggerEvent<TDStateChangeEvent<T>>(new TDStateChangeEvent<T>(this));
    }

    public virtual void RestorePreviousState()
    {
        currentState = previousState;
        if(triggerEvent)
            TDEventManager.TriggerEvent<TDStateChangeEvent<T>>(new TDStateChangeEvent<T>(this));
    }
}



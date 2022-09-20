using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;


[System.Serializable]
public class TDAIActionsList : ReorderableArray<TDAIAction>
{
}
[System.Serializable]
public class TDAITransitionsList : ReorderableArray<TDAITransition>
{
}

[System.Serializable]
public class AIState 
{
    [SerializeField]
    public string StateName;

    [Reorderable(null, "Action", null)]
    public TDAIActionsList Actions;
    [Reorderable(null, "Transitions", null)]
    public TDAITransitionsList Transitions;

    protected AIBrain _brain;


    public virtual void SetBrain(AIBrain brain)
    {
        _brain = brain;
        foreach(TDAIAction action in Actions)
        {
            if(action != null)
            {
                action.SetBrain(_brain);
            }
           
        }
        foreach(TDAITransition transition in Transitions)
        {
            if(transition != null)
            {
                if(transition.Decision != null)
                {
                    transition.Decision.SetBrain(_brain);
                } 
               
            }
           
        }
    }

    public virtual void EnterState()
    {
        foreach(TDAIAction action in Actions)
        {
            action.OnEnterAction();
        }

        foreach(TDAITransition transition in Transitions)
        {
            if(transition.Decision != null)
            {
                transition.Decision.OnEnterDecision();
            }
        }
    }

    public virtual void UpdateState()
    {
        UpdateActions();
        UpdateTrasitions();
    }

    public virtual void ExitState()
    {
        foreach (TDAIAction action in Actions)
        {
            action.OnExitAction();
        }

        foreach (TDAITransition transition in Transitions)
        {
            if (transition.Decision != null)
            {
                transition.Decision.OnExitDecision();
            }
        }
    }

    public virtual void UpdateActions()
    {
        if (Actions.Count == 0) { return; }
        for (int i = 0; i < Actions.Count; i++)
        {
            if(Actions[i] != null)
            {
                Actions[i].UpdateAction();
            }
            else
            {
                Debug.LogError("An action in " + _brain.gameObject.name + " is null");
            }
        }
    }

    public virtual void UpdateTrasitions()
    {
        if (Transitions.Count == 0) { return; }
        for (int i = 0; i < Transitions.Count; i++)
        {
            if(Transitions[i].Decision != null)
            {
                if(Transitions[i].Decision.Decide())
                {
                    if(Transitions[i].TrueState != "")
                    {
                        _brain.TransitionToState(Transitions[i].TrueState);
                    }
                }
                else
                {
                    if(Transitions[i].FalseState != "")
                    {
                        _brain.TransitionToState(Transitions[i].FalseState);
                    }
                }
            }
        }
    }
}

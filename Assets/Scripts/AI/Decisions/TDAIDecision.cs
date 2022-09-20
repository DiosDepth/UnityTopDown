using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TDAIDecision : MonoBehaviour
{
    public abstract bool Decide();
    public string Label;

    public bool DecisionInProgress { get; set; }
    protected AIBrain _brain;



    public virtual void SetBrain(AIBrain brain)
    {
        _brain = brain;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    public virtual void Initialization()
    {

    }

    public virtual void OnEnterDecision()
    {
        DecisionInProgress = true;
    }

    public virtual void OnExitDecision()
    {
        DecisionInProgress = false;
    }
}

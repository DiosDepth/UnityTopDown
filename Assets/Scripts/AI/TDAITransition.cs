using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TDAITransition 
{
    /// this transition's decision
    public TDAIDecision Decision;
    /// the state to transition to if this Decision returns true
    public string TrueState;
    /// the state to transition to if this Decision returns false
    public string FalseState;

    public virtual void Initialization()
    {
        if(Decision != null)
        {
            Decision.Initialization();
        }

    }
}

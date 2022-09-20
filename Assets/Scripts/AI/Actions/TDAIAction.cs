using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TDAIAction : MonoBehaviour
{
    public string Label;
   
    public bool ActionInProgress { get; set; }
    protected AIBrain _brain;
    // Start is called before the first frame update

    public abstract void UpdateAction();
    protected virtual void Start()
    {
        //_brain = this.gameObject.GetComponent<TDAIBrain>();
        //Initialization();
    }

    public virtual void SetBrain(AIBrain brain)
    {
        _brain = brain;
    }

    public virtual void Initialization()
    {
        
    }

    public virtual void OnEnterAction()
    {
        ActionInProgress = true;
    }

    public virtual void OnExitAction()
    {
        ActionInProgress = false;
    }
}

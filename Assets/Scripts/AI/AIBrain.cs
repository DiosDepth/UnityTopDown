using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Malee.List;

[RequireComponent(typeof(TDCharacter))]
public class AIBrain : MonoBehaviour
{
    
    public List<AIState> States;

    public bool brainActive = true;
    [Header("---DebugSettings---")]
    public bool isOnGizmos=false;
    public NavMeshAgent agent;
    public NavMeshPath path;

    public Dictionary<string, List<GameObject>> hearingDic = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<GameObject>> seeingDic = new Dictionary<string, List<GameObject>>();

    public IEnumerator moveto;

    [SerializeField]
    public AIState currentState;
    public AIState previousState;
    public TDCharacter character;

    protected TDAIDecision[] _decisions;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<TDCharacter>();
        character.Initialization();
        character.isUpdateAbility = true;
        agent = FindObjectOfType<NavMeshAgent>();
        InitializeStates();
        InitializeDecisions();
        if(States.Count >0)
        {
            TransitionToState(States[0].StateName);
        }
        //moveto = MoveTo(targetPos.position);
        //Debug.Log(path.status + " : " + path.corners.Length);

        //agent.SetDestination(targetPos.position);
        //StopCoroutine(moveto);
        //StartCoroutine(moveto);
    }
    void Update()
    {
        //aiPerception.UpdatePerception();

        UpdateState();
        //Debug.Log("Remaining distance:" + agent.remainingDistance);
        //Debug.Log("NextPosition:" + agent.nextPosition);
    }

    public void OnDrawGizmos()
    {
        if(!isOnGizmos)
        {
            return;
        }
        Gizmos.color = Color.green;
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 0; i < path.corners.Length; i++)
            {
                
                Gizmos.DrawWireSphere(path.corners[i], 0.2f);
                if(i != 0)
                {
                    Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);
                    
                }
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(agent.nextPosition, 0.1f);
        //Gizmos.DrawLine(transform.position, agent.nextPosition);

    }

    protected virtual void InitializeStates()
    {
        foreach(AIState state in States)
        {
            state.SetBrain(this);
            foreach (TDAIAction action in state.Actions)
            {
                if(action != null)
                {
                    action.Initialization();
                }         
            }
            foreach(TDAITransition transition in state.Transitions)
            {
                if(transition != null)
                {
                    transition.Initialization();
                }
               
            }
            
        }

    }

    protected virtual void InitializeDecisions()
    {
        _decisions = this.gameObject.GetComponents<TDAIDecision>();
        foreach(TDAIDecision decision in _decisions)
        {
            decision.Initialization();
        }
    }

    public virtual void UpdateState()
    {
        if (!brainActive || currentState == null)
        {
            return;
        }
        currentState.UpdateState();
    }

    public virtual void TransitionToState(string newstatename)
    {
        if(newstatename != currentState.StateName)
        {
            currentState.ExitState();
            OnExitState();
            previousState = currentState;
            currentState = FindState(newstatename);
            if(currentState != null)
            {
                currentState.EnterState();
            }
        }
    }

    protected virtual AIState FindState(string statename)
    {
        foreach(AIState state in States)
        {
            if(state.StateName == statename)
            {
                return state;
            }
        }
        Debug.LogError("You're tring to find a state " + statename + " in " + this.gameObject.name + "'s TDAIBrain, but no state of this name exists.Make sure your states are named properly, and that your transitions states match existing states");
        return null;
    }

    protected virtual void OnExitState()
    {

    }

    public virtual List<GameObject> GetHearingTargetList(string layername)
    {
        if (hearingDic.ContainsKey(layername))
        {
            return hearingDic[layername];
        }
        return null;
    }

    public virtual List<GameObject> GetSeeingTargetList(string layername)
    {
        if (seeingDic.ContainsKey(layername))
        {
            return seeingDic[layername];
        }
        return null;
    }

    public virtual GameObject GetHearingPlayerTarget(string layername = "Player")
    {
        if(hearingDic.ContainsKey("Player"))
        {
            return hearingDic["Player"][0];
        }
        return null;
    }

    public virtual GameObject GetSeeingPlayerTarget(string layername = "Player")
    {
        if (seeingDic.ContainsKey("Player"))
        {
            return hearingDic["Player"][0];
        }
        return null;
    }

}

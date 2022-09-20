using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;

[System.Serializable]
public class TDInputManager : Singleton<TDInputManager>
{
    public TDInput.TDButton attackButton;
    public TDInput.TDButton dashButton;
    public TDInput.TDButton useButton;
    public TDInput.TDButton pickupButton;
    public TDInput.TDButton pauseButton;

    [SerializeField]
    protected List<TDInput.TDButton> buttonList;

    public Vector3 inputMovement { get { return _inputMovement; } }
    public TDCharacter targetCharacter;
    public TDCharacterController targetController;
    protected Vector3 _inputMovement = Vector3.zero;
    public string attackBtnName = "attack";
    public string dashBtnName = "dash";
    public string useBtnName = "use";
    public string pickupBtnName = "pickup";
    public string pasueBtnName = "pause";
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName= "Vertical";

    [Header("Debug")]
    public bool isShowDebug = false;

    // Start is called before the first frame update

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {

    }

    public override void Initialization()
    {
        base.Initialization();
        Debug.Log("TDInputManager : " + instance.gameObject.name);
        InitializationAxis();
        InitializationButton();
        TDManagerEvent.Trigger(ManagerEventType.InitialCompleted, this.gameObject.name);
    }

    // Update is called once per frame
    public override void Update()
    {
        SetMovement();
        GetInputButtons();

        if(isShowDebug)
        {
            ShowDebugLog();
        }
    }

    private void LateUpdate()
    {
        ProcessButtonState();

    }

    public void SetMovement()
    {
        _inputMovement.x = Input.GetAxis(horizontalAxisName);
        _inputMovement.z = Input.GetAxis(verticalAxisName);
    }

    public void GetInputButtons()
    {
        int temp_count = buttonList.Count;
        for (int i = 0; i < temp_count; i++)
        {
            if (Input.GetButton(buttonList[i].buttonID))
            {
                buttonList[i].TriggerButtonPressed();
            }
            if (Input.GetButtonDown(buttonList[i].buttonID))
            {
                buttonList[i].TriggerButtonDown();
            }
            if (Input.GetButtonUp(buttonList[i].buttonID))
            {
                buttonList[i].TriggerButtonUp();
            }
        }

    }

    public void ProcessButtonState()
    {
        int temp_count = buttonList.Count;
        for (int i = 0; i < temp_count; i++)
        {
            if(buttonList[i].state.currentState == TDInput.ButtonState.ButtonDown)
            {
                StartCoroutine(DelayButtonPress(buttonList[i]));
            }
            if(buttonList[i].state.currentState == TDInput.ButtonState.ButtonUp)
            {
                StartCoroutine(DelayButtonUp(buttonList[i]));
            }
        }
    }

    IEnumerator DelayButtonPress(TDInput.TDButton btn)
    {
        yield return null;
        btn.state.ChangeState(TDInput.ButtonState.ButtonPressed);
    }

    IEnumerator DelayButtonUp(TDInput.TDButton btn)
    {
        
        yield return null;
        btn.state.ChangeState(TDInput.ButtonState.OFF);

    }

    protected void InitializationAxis()
    {
        if(horizontalAxisName == null)
        {
            horizontalAxisName = "Horizontal";
        }
        if(verticalAxisName == null)
        {
            verticalAxisName = "Vertical";
        }
        
        


    }

    protected void InitializationButton()
    {
        buttonList = new List<TDInput.TDButton>();
        buttonList.Add(attackButton = new TDInput.TDButton(attackBtnName, attackButtonDown, attackButtonPressed, attackButtonUp));
        buttonList.Add(dashButton = new TDInput.TDButton(dashBtnName, dashButtonDown, dashButtonPressed, dashButtonUp));
        buttonList.Add(useButton = new TDInput.TDButton(useBtnName, useButtonDown, useButtonPressed, useButtonUp));
        buttonList.Add(pickupButton = new TDInput.TDButton(pickupBtnName, pickupButtonDown, pickupButtonPressed, pickupButtonUp));
        buttonList.Add(pauseButton = new TDInput.TDButton(pasueBtnName, pauseButtonDown, pauseButtonPressed, pauseButtonUp));
    }


    public virtual void attackButtonDown() { attackButton.state.ChangeState(TDInput.ButtonState.ButtonDown); }
    public virtual void attackButtonPressed() { attackButton.state.ChangeState(TDInput.ButtonState.ButtonPressed); }
    public virtual void attackButtonUp() { attackButton.state.ChangeState(TDInput.ButtonState.ButtonUp); }

    public virtual void dashButtonDown() { dashButton.state.ChangeState(TDInput.ButtonState.ButtonDown); }
    public virtual void dashButtonPressed() { dashButton.state.ChangeState(TDInput.ButtonState.ButtonPressed); }
    public virtual void dashButtonUp() { dashButton.state.ChangeState(TDInput.ButtonState.ButtonUp); }

    public virtual void useButtonDown() { useButton.state.ChangeState(TDInput.ButtonState.ButtonDown); }
    public virtual void useButtonPressed() { useButton.state.ChangeState(TDInput.ButtonState.ButtonPressed); }
    public virtual void useButtonUp() { useButton.state.ChangeState(TDInput.ButtonState.ButtonUp); }

    public virtual void pickupButtonDown() { pickupButton.state.ChangeState(TDInput.ButtonState.ButtonDown); }
    public virtual void pickupButtonPressed() { pickupButton.state.ChangeState(TDInput.ButtonState.ButtonPressed); }
    public virtual void pickupButtonUp() { pickupButton.state.ChangeState(TDInput.ButtonState.ButtonUp); }

    public virtual void pauseButtonDown() { pauseButton.state.ChangeState(TDInput.ButtonState.ButtonDown); }
    public virtual void pauseButtonPressed() { pauseButton.state.ChangeState(TDInput.ButtonState.ButtonPressed); }
    public virtual void pauseButtonUp() { pauseButton.state.ChangeState(TDInput.ButtonState.ButtonUp); }

    private void ShowDebugLog()
    {
        Debug.Log("TDInputManager" + "._InputMovement =  " + _inputMovement);
        Debug.Log("TDInputManager" + ".AttackBtnState =  " + attackButton.state.currentState);
        Debug.Log("TDInputManager" + ".DashBtnState =  " + dashButton.state.currentState);
        Debug.Log("TDInputManager" + ".UseBtnState =  " + useButton.state.currentState);
    }
}

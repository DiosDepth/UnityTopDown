using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TDEnums;


namespace InputSpace
{
    public enum inputControlSchemeType
    {
        PlayerKeyBoard,
        PlayerGamePad,
    }

}
public class InputSystemManager : Singleton<InputSystemManager>
{
    public bool isTestMode;

    public UnityAction<InputAction.CallbackContext> Gameplay_InputAction_Movement;
    public UnityAction<InputAction.CallbackContext> Gameplay_InputAction_Attack;
    public UnityAction<InputAction.CallbackContext> Gameplay_InputAction_Dash;
    public UnityAction<InputAction.CallbackContext> Gameplay_InputAction_Interaction;
    public UnityAction<InputAction.CallbackContext> Gameplay_InputAction_Use;
    public UnityAction<InputAction.CallbackContext> Gameplay_InputAction_Pause;
    public UnityAction<InputAction.CallbackContext> GamePlay_InputAction_Aiming;

    public UnityAction<InputAction.CallbackContext> UI_InputAction_Move;
    public UnityAction<InputAction.CallbackContext> UI_InputAction_Submit;
    public UnityAction<InputAction.CallbackContext> UI_InputAction_Cancel;

    public Vector3 inputMovement { get { return _inputMovement; } }
    public TDCharacter targetCharacter;
    public TDCharacterController targetController;
    protected Vector3 _inputMovement = Vector3.zero;

    public PlayerInput playerInput;
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        if (isTestMode)
        {
            Initialization();
        }
    }

    public override void Initialization()
    {
        base.Initialization();
        playerInput = GetComponent<PlayerInput>();

#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager" + " : " + instance.gameObject.name, isShowDebug);
#endif 
        TDManagerEvent.Trigger(ManagerEventType.InitialCompleted, this.gameObject.name);
    }

    public void InputActionFunction_Movement(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Movement -> " + callbackContext.action.name, isShowDebug);
#endif
       
        if (Gameplay_InputAction_Movement == null) { return; }
        Gameplay_InputAction_Movement.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End InputActionFunction_Movement -> " + callbackContext.action.name, isShowDebug);
#endif
    

    }
    public void InputActionFunction_Attack(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Attack -> " + callbackContext.action.name, isShowDebug);
#endif
  
        if (Gameplay_InputAction_Attack == null) { return; }
        Gameplay_InputAction_Attack.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End  InputActionFunction_Attack -> " + callbackContext.action.name, isShowDebug);
#endif
        
    }
    public void InputActionFunction_Dash(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Dash -> " + callbackContext.action.name, isShowDebug);
#endif
        
        if (Gameplay_InputAction_Dash == null) { return; }
        Gameplay_InputAction_Dash.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End  InputActionFunction_Dash -> " + callbackContext.action.name, isShowDebug);
#endif
        
    }
    public void InputActionFunction_Interaction(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Interaction -> " + callbackContext.action.name, isShowDebug);
#endif
        
        if (Gameplay_InputAction_Interaction == null) { return; }
        Gameplay_InputAction_Interaction.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End  InputActionFunction_Interaction -> " + callbackContext.action.name, isShowDebug);
#endif
        
    }
    public void InputActionFunction_Use(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Use -> " + callbackContext.action.name, isShowDebug);
#endif
      
        if (Gameplay_InputAction_Use == null) { return; }
        Gameplay_InputAction_Use.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End  InputActionFunction_Use -> " + callbackContext.action.name, isShowDebug);
#endif
      
    }
    public void InputActionFunction_Pause(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Pause -> " + callbackContext.action.name, isShowDebug);
#endif
        
        if (Gameplay_InputAction_Pause == null) { return; }
        Gameplay_InputAction_Pause.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End  InputActionFunction_Pause -> " + callbackContext.action.name, isShowDebug);
#endif
        
    }

    public void InputActionFunction_Aiming(InputAction.CallbackContext callbackContext)
    {
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-Start InputActionFunction_Aiming -> " + callbackContext.action.name, isShowDebug);
#endif
        if (GamePlay_InputAction_Aiming == null) { return; }
        GamePlay_InputAction_Aiming.Invoke(callbackContext);
#if UNITY_EDITOR
        ExtDebug.ShowDebugLog("InputSystemManager Call-End  InputActionFunction_Aiming -> " + callbackContext.action.name, isShowDebug);
#endif
 
    }
}

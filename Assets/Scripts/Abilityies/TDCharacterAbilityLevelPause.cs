using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TDCharacterAbilityLevelPause : TDCharacterAbility
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitialAbility()
    {
        base.InitialAbility();
        InputSystemManager.instance.Gameplay_InputAction_Pause += HandleInput;
    }

    public override void HandleInput(InputAction.CallbackContext callbackContext)
    {
        base.HandleInput(callbackContext);

        if (callbackContext.phase == InputActionPhase.Performed)
        {
            GamePaused();
        }
    }

    public void GamePaused()
    {
        TDLevelEvent.Trigger(TDEnums.LevelState.LevelPause,SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().buildIndex,null);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        InputSystemManager.instance.Gameplay_InputAction_Pause -= HandleInput;
    }
}

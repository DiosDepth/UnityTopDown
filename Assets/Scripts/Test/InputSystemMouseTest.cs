using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemMouseTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 mousePos;
    public PlayerInput playerinput;
    void Start()
    {
        playerinput = GetComponent<PlayerInput>();
        InputSystemManager.instance.GamePlay_InputAction_Aiming += HandleInput;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = playerinput.actions.FindActionMap("GamePlay").FindAction("Aiming").ReadValue<Vector2>();
        Debug.Log("mouse pos : " + mousePos);
    }

    public void HandleInput(InputAction.CallbackContext callbackContext)
    {
       Debug.Log("Aiming event call : " + callbackContext.ReadValue<Vector2>());
    }
}

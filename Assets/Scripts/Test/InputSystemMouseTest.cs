using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemMouseTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 mousePos;
    public PlayerInput playerinput;
    public Vector3 mousePoing;
    Ray cameraray;
    public RaycastHit hit;
    public LayerMask mask = 0;
    void Start()
    {
        playerinput = GetComponent<PlayerInput>();
        InputSystemManager.instance.GamePlay_InputAction_Aiming += HandleInput;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Ray origin = " + cameraray.origin +  "  |   Ray dir = " + cameraray.direction);
        Debug.DrawLine(cameraray.origin, cameraray.origin + cameraray.direction * 100, Color.green);

    }

    public void HandleInput(InputAction.CallbackContext callbackContext)
    {
        cameraray = Camera.main.ScreenPointToRay(callbackContext.ReadValue<Vector2>());
        mousePoing = Camera.main.ScreenToWorldPoint(callbackContext.ReadValue<Vector2>());
        mousePoing.z = 0;
     /*   if(Physics.Raycast(cameraray, out hit, 100f, mask))
        {
            Debug.Log("Hit Point = " + hit.point);
        }*/
     //Debug.Log("Aiming event call : " + callbackContext.ReadValue<Vector2>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePoing, 1f);
    }

}

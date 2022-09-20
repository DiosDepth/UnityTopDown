using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class InputSystemTest : MonoBehaviour
{
    public Vector3 moveDirection;
    public PlayerInputAsset playerInputAsset;
    public InputActionMap inputActionMap;
    public InputAction[] inputAction;
    public InputActionAsset inputActionAsset;
    public string releventPath;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInputAsset = new PlayerInputAsset();
    }
    void Start()
    {
        releventPath = Application.dataPath;

        //TDDataManager.instance.LoadResAsync<InputActionAsset>("InputAssets/PlayerInputAsset", (asset) =>
        // {
        //     if (asset != null)
        //     {
        //         inputActionAsset = asset as InputActionAsset;
        //     }
        //     else
        //     {
        //         Debug.Log("Asset is null");
        //     }
        // });

        inputActionAsset = playerInputAsset.asset;


        inputActionMap = inputActionAsset.FindActionMap("Gameplay");
        inputAction = inputActionMap.actions.ToArray();

        for (int i = 0; i < inputAction.Length; i++)
        {
            Debug.Log("-----Start Looping bindings from" + inputAction[i].name +" in count " + inputAction[i].bindings.Count+ "-----");
            for (int n = 0; n < inputAction[i].bindings.Count; n++)
            {
                Debug.Log(" --------> binding : " + inputAction[i].bindings[n].name + " [ composite = " + inputAction[i].bindings[n].isComposite + " ] [ part of composite = " + inputAction[i].bindings[n].isPartOfComposite + " ] [ control scheme = " + inputAction[i].bindings[n].groups);
            }
            Debug.Log("----------------------------------->End Looping bindings from" + inputAction[i].name );

        }
        //inputActionAsset = Resources.Load(@"InputAssets/PlayerInputAsset") as InputActionAsset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
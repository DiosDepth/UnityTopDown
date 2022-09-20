using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;

public class TDCameraManager : Singleton<TDCameraManager>
{
    public Camera currentActiveCamera;
    public TDCameraController currentActiveCameraController;

    public override void Update()
    {
        base.Update();

    }

    public override void Initialization()
    {
        base.Initialization();
        Debug.Log("TDCameraManager : " + instance.gameObject.name);
        currentActiveCamera = Camera.main;
        if (currentActiveCamera != null)
        {
            currentActiveCameraController = currentActiveCamera.GetComponent<TDCameraController>();

        }
        //currentActiveCameraController.InitializationCameraPosition();
        TDManagerEvent.Trigger(ManagerEventType.InitialCompleted, this.gameObject.name);
    }

    public void ProcessCamera()
    {
        currentActiveCameraController.InitializationCameraPosition();
        currentActiveCameraController.ProcessCamera();
    }

    public void UnProcessCamera()
    {
        currentActiveCameraController.UnProcessCamera();
    }
}

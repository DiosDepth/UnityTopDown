using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPlanType
{
    XY,
    XZ,
    YZ,
}

public class TDCameraController : MonoBehaviour
{
    public CameraPlanType cameraplan = CameraPlanType.XY;
    public GameObject target;
    public float maxMoveSpeed = 0.2f;
    public float moveTime = 0.3f;
    

    public Vector3 cameraArmRotation = new Vector3(0, 0, 90);
    public float cameraDistance = 10;
    private Vector3 desiredPosition;
    private Vector3 currentVelocity;
    private bool isProcessCamera;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isProcessCamera)
        {
            CalculateTargetPosition(cameraplan);
            UpdateCameraPosition();
        }
        
    }
    
    private void LateUpdate()
    {
        
    }
    private void FixedUpdate()
    {
        if (isProcessCamera)
        {
            
        }
    }

    public void InitializationCameraPosition()
    {
        target = GameObject.Find("Player");
        if(target == null)
        {
            target = TDLevelManager.instance.currentPlayer.gameObject;
            if (target == null)
            {
                Debug.Log("TDCameraController : " + "can't find any player in the scene");
                return;
            }
        }
        desiredPosition = new Vector3(0, 0, cameraDistance);
        desiredPosition = Quaternion.Euler(cameraArmRotation) * desiredPosition;
        desiredPosition += target.transform.position;
        transform.position = desiredPosition;
        transform.LookAt(target.transform);
    }

    private void CalculateTargetPosition(CameraPlanType m_playtype)
    {
        switch (m_playtype)
        {
            case CameraPlanType.XY:
                desiredPosition.x = target.transform.position.x;
                desiredPosition.y = target.transform.position.y;
                break;
            case CameraPlanType.XZ:
                desiredPosition.x = target.transform.position.x;
                desiredPosition.z = target.transform.position.z;
                break;
            case CameraPlanType.YZ:
                desiredPosition.y = target.transform.position.y;
                desiredPosition.z = target.transform.position.z;
                break;
        }
    
    }

    private void UpdateCameraPosition()
    {
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, moveTime, maxMoveSpeed);
       
    }

    public void ProcessCamera()
    {
        isProcessCamera = true;
    }
    public void UnProcessCamera()
    {
        isProcessCamera = false;
    }

}

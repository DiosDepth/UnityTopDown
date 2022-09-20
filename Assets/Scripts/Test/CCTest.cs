using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTest : MonoBehaviour
{
    public CharacterController cc;
    public GameObject target;
    // Start is called before the first frame update
    private void Awake()
    {
        cc = target.GetComponent<CharacterController>();
        Debug.Log("CCTest : " + "Awake");
    }
    void Start()
    {
        Debug.Log("CCTest : " + "Start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        cc.Move(Vector3.zero);
        Debug.Log("CCTest : " + "Update");
    }
}

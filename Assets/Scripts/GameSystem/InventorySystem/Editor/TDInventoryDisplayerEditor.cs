using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TDInventoryDisplayer))]
public class TDInventoryDisplayerEditor : Editor
{
    private TDInventoryDisplayer _target;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {

        _target = (TDInventoryDisplayer)target;
        _target.Initialization();

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();


        if (GUILayout.Button("CreateSlot"))
        {
            Debug.Log("CreateSlot");
            _target.ResizeLayoutGroup(_target.slotsize.x,_target.slotsize.y);
        }

        if (GUILayout.Button("ResetNavigation"))
        {
            Debug.Log("ResetNavigation");
            _target.SetNavigation();
        }
        serializedObject.ApplyModifiedProperties();
    }
}

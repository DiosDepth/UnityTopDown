using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIFactory))]
public class AIFactoryEditor : Editor   
{
    private AIFactory _target;
    public SerializedProperty _waveInfo;
    public SerializedProperty _ai;
    
   

     void OnEnable()
    {
        _target = (AIFactory)target;
        _waveInfo = serializedObject.FindProperty("waveInfo");
     

    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_waveInfo, true);

    

        if (GUILayout.Button("SpawnAI"))
        {
            Debug.Log("SpawnAI");
            _target.WaveStart();
        }
       serializedObject.ApplyModifiedProperties();
    }
}

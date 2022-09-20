using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Malee.List;

[CustomEditor(typeof(AIBrain))]
public class TDAIBrainEditor : Editor
{
    ReorderableList _stateList;
    SerializedProperty _currentState;

    public void OnEnable()
    {
        _stateList = new Malee.List.ReorderableList(serializedObject.FindProperty("States"),true,true,true);
        _stateList.elementNameProperty = "States";
        _stateList.elementDisplayType = Malee.List.ReorderableList.ElementDisplayType.Expandable;
        _currentState = serializedObject.FindProperty("currentState").FindPropertyRelative("StateName");

    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_currentState);
        _stateList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

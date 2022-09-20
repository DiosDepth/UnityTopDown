using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


//[CustomEditor(typeof(TDAIActionListTest))]
public class TDAIActionListTestEditor : Editor
{
    ReorderableList _stringlist;

    ReorderableList _actionlist;

    ReorderableList _arraylist;

    public void OnEnable()
    {
        _stringlist = new ReorderableList(serializedObject, serializedObject.FindProperty("stringlist"), true, true, true, true);

        _stringlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = _stringlist.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);
        };

        _actionlist = new ReorderableList(serializedObject, serializedObject.FindProperty("actionlist"), true, true, true, true);

        _actionlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.x += 15;
            rect.width -= 30;
            SerializedProperty element = _actionlist.serializedProperty.GetArrayElementAtIndex(index);

            //_arraylist = new ReorderableList(element.serializedObject, element.FindPropertyRelative("array"), true, true, true, true);

            //_arraylist.drawElementCallback = (Rect arrayrect, int arrayindex, bool arrayisActive, bool arrayisFocused) =>
            //{
            //    SerializedProperty arrayelement = _arraylist.serializedProperty.GetArrayElementAtIndex(arrayindex);
            //    EditorGUI.PropertyField(rect, arrayelement);
            //};
            //_arraylist.DoList(rect);
            EditorGUI.PropertyField(rect, element, true);
        };
        _actionlist.elementHeightCallback = (int index) => 
        {
            float height = EditorGUIUtility.singleLineHeight;
            SerializedProperty element = _actionlist.serializedProperty.GetArrayElementAtIndex(index);
            if(element.isExpanded)
            {
                height = 2*EditorGUIUtility.singleLineHeight;
                if (element.FindPropertyRelative("array").isExpanded)
                {
                    height += (element.FindPropertyRelative("array").arraySize + 2) * EditorGUIUtility.singleLineHeight;
                }
            }
            else
            {
                height = EditorGUIUtility.singleLineHeight;
            }

            return height;
        };


        


    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        //_stringlist.DoLayoutList();
       // _actionlist.DoLayoutList();
       // _arraylist.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

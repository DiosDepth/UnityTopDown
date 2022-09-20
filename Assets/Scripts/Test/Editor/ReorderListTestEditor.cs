using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

//[CustomEditor(typeof(ReorderListTest))]
public class ReorderListTestEditor :Editor
{
    ReorderableList _statelist;
    ReorderableList _actionlist;
    ReorderableList _transitionlist;

    SerializedObject _stateObj;
    SerializedProperty _TDAIstate;
    float lineHeight;
    float lineHeightSpace;
    public void OnEnable()
    {
        if(target == null)
        {
            return;
        }
        lineHeight = EditorGUIUtility.singleLineHeight;
        lineHeightSpace = lineHeight + 2;


        _statelist = new ReorderableList(this.serializedObject, serializedObject.FindProperty("StateList"), true, true, true, true);
        

        _statelist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
        {
            rect.x += 15;
            SerializedProperty element = _statelist.serializedProperty.GetArrayElementAtIndex(index);
           
           /* EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), element.FindPropertyRelative("StateName").stringValue);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace), rect.width - 30, lineHeight), element.FindPropertyRelative("Actions"));*/

            //EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace), rect.width - 30, lineHeight), element);
            int i = 1;
            while (element.NextVisible(false))
            {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width-50, lineHeight), element,true);
                i++;
            }

            //EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("StateName"), true);
            //EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * 2), rect.width, lineHeight), element.FindPropertyRelative("Actions"), true);
            //EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * 3), rect.width, lineHeight), element.FindPropertyRelative("Transitions"), true);




            /* {
                 EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace), rect.width, lineHeight), element, true);
             }*/


        };
        _statelist.elementHeightCallback = (int index) =>
         {
             float height = 0;

             SerializedProperty element = _statelist.serializedProperty.GetArrayElementAtIndex(index);
           
             SerializedProperty propertyIterator = serializedObject.GetIterator();

             int i = 1;
             while(element.NextVisible(true))
             {
                 i++;
             }
             height = lineHeightSpace * i;
             return height;
         };

    }
    
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        _statelist.DoLayoutList();
        

        this.serializedObject.ApplyModifiedProperties();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(AudioManager))]
public class AudioEngineEditor : Editor
{
    private AudioManager audioManager;
    private ReorderableList soundList;
    
    public void OnEnable()
    {
        audioManager = (AudioManager)target;
        soundList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("eventName"),
            true,true,true,true);

        soundList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = soundList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        soundList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("eventName"));
    }
}

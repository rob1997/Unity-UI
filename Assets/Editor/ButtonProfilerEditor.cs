using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonProfiler))]
public class ButtonProfilerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ButtonProfiler buttonProfiler = (ButtonProfiler) target;

        if (buttonProfiler == null) return;

        SerializedProperty buttonProfileProperty = serializedObject.FindProperty(nameof(buttonProfiler.buttonProfile));

        //If null apply default profile
        if (buttonProfileProperty.objectReferenceValue == null) buttonProfileProperty.objectReferenceValue = 
            AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath).defaultButtonProfile;


        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Profile", GUILayout.MaxWidth(50));
        
        buttonProfileProperty.objectReferenceValue = EditorGUILayout.ObjectField(buttonProfileProperty.objectReferenceValue, typeof(ButtonProfile), false);
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        if (buttonProfileProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please assign a Color Palette", MessageType.Error);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Profile Type", GUILayout.MaxWidth(75));
        
        serializedObject.FindProperty(nameof(buttonProfiler.buttonProfileType)).enumValueIndex = 
            (int) (ButtonProfile.ButtonProfileType) EditorGUILayout.EnumPopup(buttonProfiler.buttonProfileType);
        
        EditorGUILayout.EndHorizontal();
        
        serializedObject.ApplyModifiedProperties();
        
        buttonProfiler.Apply();
    }
}

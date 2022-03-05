using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FontProfiler))]
public class FontProfilerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FontProfiler fontProfiler = (FontProfiler) target;

        if (fontProfiler == null) return;

        SerializedProperty fontProfileProperty = serializedObject.FindProperty(nameof(fontProfiler.fontProfile));
        SerializedProperty fontGroupProperty = serializedObject.FindProperty(nameof(fontProfiler.fontGroup));

        //If null apply default profile
        if (fontProfileProperty.objectReferenceValue == null) fontProfileProperty.objectReferenceValue = 
            AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath).defaultFontProfile;

        EditorGUILayout.PropertyField(fontProfileProperty);
        
        if (fontProfileProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please assign a Font Profile", MessageType.Error);
            return;
        }
        
        EditorGUILayout.Space(5);
        
        if (fontGroupProperty.objectReferenceValue == null) fontGroupProperty.objectReferenceValue = 
            AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath).defaultFontGroup;

        EditorGUILayout.PropertyField(fontGroupProperty);
        
        if (fontGroupProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please assign a Font Group", MessageType.Error);
            return;
        }
        
        EditorGUILayout.Space(5);

        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(FontProfiler.fontProfileType)));
        
        serializedObject.ApplyModifiedProperties();
        
        fontProfiler.Apply();
    }
}

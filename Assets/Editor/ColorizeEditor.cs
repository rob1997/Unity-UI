using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Colorize)), CanEditMultipleObjects]
public class ColorizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(5);
        
        Colorize colorize = (Colorize) target;
        if (colorize == null) return;
        
        SerializedProperty paletteProperty = serializedObject.FindProperty(nameof(colorize.colorPalette));
        
        //If null apply default palette
        if (paletteProperty.objectReferenceValue == null) paletteProperty.objectReferenceValue = AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath).defaultColorPalette;


        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Palette", GUILayout.MaxWidth(50));
        
        paletteProperty.objectReferenceValue = EditorGUILayout.ObjectField(paletteProperty.objectReferenceValue, typeof(ColorPalette), false);
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        if (paletteProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please assign a Color Palette", MessageType.Error);
            return;
        }
        
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Ui Color", GUILayout.MaxWidth(50));
        
        serializedObject.FindProperty(nameof(colorize.colorType)).enumValueIndex = (int) (ColorPalette.ColorType) EditorGUILayout.EnumPopup(colorize.colorType);
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Overlay", GUILayout.MaxWidth(50));
        
        serializedObject.FindProperty(nameof(colorize.isOverlay)).boolValue = EditorGUILayout.Toggle(colorize.isOverlay);
        
        EditorGUILayout.EndHorizontal();
        
        serializedObject.ApplyModifiedProperties();
        
        colorize.Apply();
    }
}

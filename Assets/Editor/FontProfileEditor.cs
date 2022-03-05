using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FontProfile))]
public class FontProfileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FontProfile fontProfile = (FontProfile) target;
        
        if (target == null) return;

        EditorGUILayout.BeginVertical();
        
        SerializedProperty maxFontSizeProperty = serializedObject.FindProperty(nameof(fontProfile.maxFontSize));
        EditorGUILayout.PropertyField(maxFontSizeProperty);

        List<FontProfile.FontProfileType> allProfiles =
            Utils.GetEnumValues<FontProfile.FontProfileType>().ToList();

        SerializedProperty profilesListProperty = serializedObject.FindProperty(nameof(fontProfile.profiles)).FindPropertyRelative("list");

        for (int i = 0; i < profilesListProperty.arraySize; i++)
        {
            SerializedProperty keyValuePairProperty = profilesListProperty.GetArrayElementAtIndex(i);
            
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");

            if (keyProperty.enumValueIndex == -1)
            {
                profilesListProperty.DeleteArrayElementAtIndex(i);
                continue;
            }
            
            FontProfile.FontProfileType fontProfileType = fontProfile.profiles.Keys.ToArray()[keyProperty.enumValueIndex];

            allProfiles.Remove(fontProfileType);
            
            GUILayout.BeginVertical(Utils.GetDisplayName(fontProfileType.ToString()), "window");
            
            SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");

            SerializedProperty minSizeValueProperty = valueProperty.FindPropertyRelative(nameof(FontProfile.UiFont.minSize));
            SerializedProperty maxSizeValueProperty = valueProperty.FindPropertyRelative(nameof(FontProfile.UiFont.maxSize));

            minSizeValueProperty.floatValue = EditorGUILayout.Slider(Utils.GetDisplayName(nameof(FontProfile.UiFont.minSize)), minSizeValueProperty.floatValue, 0f, maxFontSizeProperty.floatValue);
            maxSizeValueProperty.floatValue = EditorGUILayout.Slider(Utils.GetDisplayName(nameof(FontProfile.UiFont.maxSize)), maxSizeValueProperty.floatValue, minSizeValueProperty.floatValue, maxFontSizeProperty.floatValue);

            EditorGUILayout.PropertyField(valueProperty.FindPropertyRelative(nameof(FontProfile.UiFont.fontWeight)));
            EditorGUILayout.PropertyField(valueProperty.FindPropertyRelative(nameof(FontProfile.UiFont.fontColorType)));
            EditorGUILayout.PropertyField(valueProperty.FindPropertyRelative(nameof(FontProfile.UiFont.isOverlay)));
            
            GUILayout.EndVertical();
            
            EditorGUILayout.Space(15f);
        }
        
        for (int i = 0; i < allProfiles.Count; i++)
        {
            profilesListProperty.arraySize++;

            profilesListProperty.GetArrayElementAtIndex(profilesListProperty.arraySize - 1).FindPropertyRelative("Key")
                .enumValueIndex = (int) allProfiles[i];
        }
        
        EditorGUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
    }
}

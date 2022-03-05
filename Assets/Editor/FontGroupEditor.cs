using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FontGroup))]
public class FontGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FontGroup fontGroup = (FontGroup) target;
        
        if (target == null) return;

        EditorGUILayout.BeginVertical();

        List<FontProfile.FontWeightType> allProfiles =
            Utils.GetEnumValues<FontProfile.FontWeightType>().ToList();

        SerializedProperty profilesListProperty = serializedObject.FindProperty(nameof(fontGroup.@group)).FindPropertyRelative("list");

        for (int i = 0; i < profilesListProperty.arraySize; i++)
        {
            SerializedProperty keyValuePairProperty = profilesListProperty.GetArrayElementAtIndex(i);
            
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");

            if (keyProperty.enumValueIndex == -1)
            {
                profilesListProperty.DeleteArrayElementAtIndex(i);
                continue;
            }
            
            FontProfile.FontWeightType fontWeightType = fontGroup.group.Keys.ToArray()[i];

            allProfiles.Remove(fontWeightType);
            
            GUILayout.BeginVertical("window");

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(Utils.GetDisplayName(fontWeightType.ToString()), GUILayout.MaxWidth(75));
            
            SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");

            valueProperty.objectReferenceValue = EditorGUILayout.ObjectField(valueProperty.objectReferenceValue, typeof(TMP_FontAsset), false);
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            EditorGUILayout.Space(5f);
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

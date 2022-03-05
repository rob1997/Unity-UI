using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonProfile))]
public class ButtonProfileEditor : Editor
{
    private int[] _selectedToolbar;

    private void OnEnable()
    {
        _selectedToolbar = new int[Utils.GetEnumValues<ButtonProfile.ButtonProfileType>().Length];
    }

    public override void OnInspectorGUI()
    {
        ButtonProfile buttonProfile = (ButtonProfile) target;
        
        if (target == null) return;

        EditorGUILayout.BeginVertical();

        List<ButtonProfile.ButtonProfileType> allProfiles =
            Utils.GetEnumValues<ButtonProfile.ButtonProfileType>().ToList();

        SerializedProperty profilesListProperty = serializedObject.FindProperty(nameof(buttonProfile.profiles)).FindPropertyRelative("list");

        for (int i = 0; i < profilesListProperty.arraySize; i++)
        {
            SerializedProperty keyValuePairProperty = profilesListProperty.GetArrayElementAtIndex(i);
            
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");

            if (keyProperty.enumValueIndex == -1)
            {
                profilesListProperty.DeleteArrayElementAtIndex(i);
                continue;
            }
            
            ButtonProfile.ButtonProfileType buttonProfileType = buttonProfile.profiles.Keys.ToArray()[keyProperty.enumValueIndex];

            allProfiles.Remove(buttonProfileType);
            
            GUILayout.BeginVertical(Utils.GetDisplayName(buttonProfileType.ToString()), "window");
            
            SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");

            EditorGUILayout.PropertyField(valueProperty.FindPropertyRelative(nameof(ButtonProfile.UiButton.colorType)));
            
            SerializedProperty buttonStatesListProperty = valueProperty.FindPropertyRelative(nameof(ButtonProfile.UiButton.buttonStates)).FindPropertyRelative("list");

            _selectedToolbar[i] = GUILayout.Toolbar(_selectedToolbar[i], 
                Array.ConvertAll(Utils.GetEnumValues<ButtonProfile.ButtonStateType>(), item => item.ToString()));

            SerializedProperty buttonStateValueProperty = buttonStatesListProperty
                .GetArrayElementAtIndex(_selectedToolbar[i]).FindPropertyRelative("Value");

            bool isFill = buttonStateValueProperty.FindPropertyRelative(nameof(ButtonProfile.ButtonState.isFill)).boolValue;
            
            foreach (SerializedProperty buttonStateProperty in buttonStateValueProperty)
            {
                switch (buttonStateProperty.name)
                {
                    case nameof(ButtonProfile.ButtonState.uiColor):
                        continue;
                    case nameof(ButtonProfile.ButtonState.borderWidth):
                        if (isFill) continue;
                        break;
                }
                
                EditorGUILayout.PropertyField(buttonStateProperty);
            }
            
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

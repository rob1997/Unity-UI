using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UiManager))]
public class UiManagerEditor : Editor
{
    private bool _menuRefFoldout;
    
    public override void OnInspectorGUI()
    {
        UiManager uiManager = (UiManager) target;
        if (uiManager == null) return;

        GUILayout.BeginVertical();
        
        DrawDefaultInspector();

        EditorGUILayout.Space(5);
        
        _menuRefFoldout = EditorGUILayout.Foldout(_menuRefFoldout, Utils.GetDisplayName(nameof(uiManager.menuReferences)));

        EditorGUILayout.Space(10);
        
        SerializedProperty paletteProperty = serializedObject.FindProperty(nameof(uiManager.menuReferences)).FindPropertyRelative("list");
        
        List<UiConstants.UiMenu> menuTypes = Utils.GetEnumValues<UiConstants.UiMenu>().ToList();
        
        for (int i = 0; i < paletteProperty.arraySize; i++)
        {
            SerializedProperty keyValuePairProperty = paletteProperty.GetArrayElementAtIndex(i);
                
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");

            //If enum doesn't exist anymore
            if (keyProperty.enumValueIndex == -1)
            {
                paletteProperty.DeleteArrayElementAtIndex(i);
                continue;
            }

            UiConstants.UiMenu uiMenu = (UiConstants.UiMenu) keyProperty.enumValueIndex;

            menuTypes.Remove(uiMenu);

            if (_menuRefFoldout)
            {
                GUILayout.BeginVertical(Utils.GetDisplayName(uiMenu.ToString()), "window");
            
                SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");

                float cachedLabelWidth = EditorGUIUtility.labelWidth;
                //can't be zero, just resets to original width
                EditorGUIUtility.labelWidth = .01f;
                EditorGUILayout.PropertyField(valueProperty, GUIContent.none);
                EditorGUIUtility.labelWidth = cachedLabelWidth;
            
                GUILayout.EndVertical();
            
                EditorGUILayout.Space(5);
            }
        }

        for (int i = 0; i < menuTypes.Count; i++)
        {
            paletteProperty.arraySize++;

            paletteProperty.GetArrayElementAtIndex(paletteProperty.arraySize - 1).FindPropertyRelative("Key")
                .enumValueIndex = (int) menuTypes[i];
        }
        
        GUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
    }
}

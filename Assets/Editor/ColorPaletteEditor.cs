using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor
{
    private int[] _selectedToolbar;

    private void OnEnable()
    {
        _selectedToolbar = new int[Utils.GetEnumValues<ColorPalette.ColorType>().Length];
    }

    public override void OnInspectorGUI()
    {
        ColorPalette palette = (ColorPalette) target;
        
        if (palette == null) return;

        GUILayout.BeginVertical();
        
        SerializedProperty paletteProperty = serializedObject.FindProperty(nameof(palette.palette)).FindPropertyRelative("list");
        
        List<ColorPalette.ColorType> allColors = Utils.GetEnumValues<ColorPalette.ColorType>().ToList();
        
        for (int i = 0; i < paletteProperty.arraySize; i++)
        {
            SerializedProperty keyValuePairProperty = paletteProperty.GetArrayElementAtIndex(i);
                
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");

            if (keyProperty.enumValueIndex == -1)
            {
                paletteProperty.DeleteArrayElementAtIndex(i);
                continue;
            }

            ColorPalette.ColorType colorType = (ColorPalette.ColorType) keyProperty.enumValueIndex;

            allColors.Remove(colorType);
            
            GUILayout.BeginVertical(Utils.GetDisplayName(colorType.ToString()), "window");
            
            SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");
                
            _selectedToolbar[i] = GUILayout.Toolbar(_selectedToolbar[i], new []{"Main", "Overlay"});

            switch (_selectedToolbar[i])
            {
                case 0:
                    EditorGUILayout.PropertyField(valueProperty.FindPropertyRelative("main"), GUIContent.none);
                    break;
                case 1:
                    EditorGUILayout.PropertyField(valueProperty.FindPropertyRelative("overlay"), GUIContent.none);
                    break;
            }
            
            GUILayout.EndVertical();
            
            EditorGUILayout.Space(5);
        }

        for (int i = 0; i < allColors.Count; i++)
        {
            paletteProperty.arraySize++;

            paletteProperty.GetArrayElementAtIndex(paletteProperty.arraySize - 1).FindPropertyRelative("Key")
                .enumValueIndex = (int) allColors[i];
        }
        
        GUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
    }
}
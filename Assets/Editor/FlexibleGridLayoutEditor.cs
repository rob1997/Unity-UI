using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlexibleGridLayout))]
public class FlexibleGridLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FlexibleGridLayout flexibleGridLayout = (FlexibleGridLayout) target;
        if (flexibleGridLayout == null) return;

        SerializedProperty paddingProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.padding));
        EditorGUILayout.PropertyField(paddingProperty);
        
        SerializedProperty fitTypeProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.fitType));
        EditorGUILayout.PropertyField(fitTypeProperty);

        EditorGUI.BeginDisabledGroup(fitTypeProperty.enumValueIndex != (int) FlexibleGridLayout.FitType.FixedRows);
        
        SerializedProperty rowsProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.rows));
        rowsProperty.intValue = EditorGUILayout.IntField(nameof(flexibleGridLayout.rows), rowsProperty.intValue <= 0 ? 1 : rowsProperty.intValue);
        
        EditorGUI.EndDisabledGroup();
        
        EditorGUI.BeginDisabledGroup(fitTypeProperty.enumValueIndex != (int) FlexibleGridLayout.FitType.FixedColumns);
        
        SerializedProperty columnsProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.columns));
        columnsProperty.intValue = EditorGUILayout.IntField(nameof(flexibleGridLayout.columns), columnsProperty.intValue <= 0 ? 1 : columnsProperty.intValue);
        
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(flexibleGridLayout.spacing)));

        SerializedProperty fitXProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.fitX));
        SerializedProperty fitYProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.fitY));
        
        EditorGUILayout.PropertyField(fitXProperty);
        EditorGUILayout.PropertyField(fitYProperty);
        
        EditorGUI.BeginDisabledGroup(fitXProperty.boolValue);
        
        SerializedProperty cellWidthProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.cellSize)).FindPropertyRelative("x");
        cellWidthProperty.floatValue = EditorGUILayout.FloatField("Cell Width", cellWidthProperty.floatValue);
        
        EditorGUI.EndDisabledGroup();
        
        EditorGUI.BeginDisabledGroup(fitYProperty.boolValue);
        
        SerializedProperty cellHeightProperty = serializedObject.FindProperty(nameof(flexibleGridLayout.cellSize)).FindPropertyRelative("y");
        cellHeightProperty.floatValue = EditorGUILayout.FloatField("Cell Height", cellHeightProperty.floatValue);
        
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(flexibleGridLayout.centerX)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(flexibleGridLayout.centerY)));
        
        serializedObject.ApplyModifiedProperties();
    }
}

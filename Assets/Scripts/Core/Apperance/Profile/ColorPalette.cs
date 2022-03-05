using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ColorPalette), menuName = "Ui-Framework/Color/Palette", order = 0)]
public class ColorPalette : ScriptableObject
{
    [Serializable]
    public struct UiColor
    {
        public static UiColor DefaultUiColor = new UiColor
        {
            main = Color.white,
            overlay = Color.white
        };
        
        public Color main;
        public Color overlay;
    }

    public enum ColorType
    {
        Primary,
        Secondary,
        Background,
        Surface,
        Error,
        Warning,
        Info,
        Success,
    }
    
    public GenericDictionary<ColorType, UiColor> palette = GenericDictionary<ColorType, UiColor>
        .ToGenericDictionary(Utils.GetEnumValues<ColorType>().ToDictionary(c => c, c => new UiColor()));
    
    public static bool GetUiColor(ColorType colorType, out ColorPalette.UiColor uiColor)
    {
        uiColor = UiColor.DefaultUiColor;
        
#if UNITY_EDITOR
        try
        {
            bool foundColor = AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath)
                .defaultColorPalette.palette.TryGetValue(colorType, out uiColor);

            return foundColor;
        }
        
        catch (Exception e)
        {
            Debug.LogWarning($"Exception Fetching Color {e}");
        }
#endif
        if (UiManager.Instance != null)
        {
            try
            {
                bool foundColor = UiManager.Instance.uiPreferences.defaultColorPalette.palette.TryGetValue(colorType,
                    out uiColor);

                return foundColor;
            }
            
            catch (Exception e)
            {
                Debug.LogWarning($"Exception Fetching Color {e}");
            }
        }
        
        else Debug.LogError($"{nameof(UiManager)} not initialized");

        return false;
    }
}
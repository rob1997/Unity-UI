using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TMP_Text))]
public class FontProfiler : MonoBehaviour
{
    public FontProfile.FontProfileType fontProfileType;
    public FontProfile fontProfile;
    public FontGroup fontGroup;

    private TMP_Text _text;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        fontProfile = AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath).defaultFontProfile;
        fontGroup = AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath).defaultFontGroup;
#endif
        if (UiManager.Instance != null)
        {
            fontProfile = UiManager.Instance.uiPreferences.defaultFontProfile;
            fontGroup = UiManager.Instance.uiPreferences.defaultFontGroup;
        }
        
        Apply();
    }

#if UNITY_EDITOR
    private void Update()
    {
        Apply();
    }
#endif
    
    public void Apply()
    {
        if (_text == null) _text = GetComponent<TMP_Text>();

        if (fontProfile.profiles.TryGetValue(fontProfileType, out FontProfile.UiFont uiFont))
        {
            _text.enableAutoSizing = true;
            
            _text.fontSizeMin = uiFont.minSize;
            _text.fontSizeMax = uiFont.maxSize;

            if (ColorPalette.GetUiColor(uiFont.fontColorType, out ColorPalette.UiColor fontColor))
            {
                _text.color = uiFont.isOverlay ? fontColor.overlay : fontColor.main;
            }
            
            else Debug.LogError($"Error fetching color from Palette");

            if (fontGroup.group.TryGetValue(uiFont.fontWeight, out TMP_FontAsset font))
            {
                if (font == null) Debug.LogError($"Can't Find Specified Weight {uiFont.fontWeight} in FontGroup");
                
                _text.font = font;
            }
            
            else Debug.LogError($"Can't Find Specified Weight {uiFont.fontWeight} in FontGroup");
        }

        else Debug.LogError($"Can't Find Specified Font Profile Type {fontProfileType} in FontProfile");
    }
}

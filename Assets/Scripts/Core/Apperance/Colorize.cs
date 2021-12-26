using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Graphic))]
public class Colorize : MonoBehaviour
{
    [HideInInspector] public ColorPalette.ColorType colorType;
    
    [HideInInspector] public bool isOverlay;
    
    [HideInInspector] public ColorPalette colorPalette;
    
    private Graphic _graphic;
    
    private void Awake()
    {
        _graphic = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        if (colorPalette == null)
        {
            try
            {
#if UNITY_EDITOR
                colorPalette = AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath)
                    .defaultColorPalette;
#endif
                if (UiManager.Instance != null) colorPalette = UiManager.Instance.uiPreferences.defaultColorPalette;
            }
            
            catch (Exception e)
            {
                Debug.LogError(e);
            }
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
        if (colorPalette == null)
        {
            Debug.LogError("Can't Apply, No Palette Found");
            return;
        }

        if (colorPalette.palette.TryGetValue(colorType, out ColorPalette.UiColor uiColor))
        {
            if (_graphic == null) _graphic = GetComponent<Graphic>();
            
            _graphic.color = isOverlay ? uiColor.overlay : uiColor.main;
        }
        
        else
        {
            Debug.LogError($"{colorType} not Found in Palette");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(UiPreferences), menuName = "Ui-Framework/Preference", order = 0)]
public class UiPreferences : ScriptableObject
{
    public float cacheTimeOutSeconds = 5f;
    
    public Transition defaultTransition;
    
    public float defaultFullTransitionTime = .5f;
    
    public ColorPalette defaultColorPalette;
    public ButtonProfile defaultButtonProfile;
    
    public FontProfile defaultFontProfile;
    public FontGroup defaultFontGroup;

    public List<UiConstants.UiMenu> startingMenus;
    
    public static readonly string DefaultUiPreferencePath = "Assets/Resources/UiPreferences.asset";
}

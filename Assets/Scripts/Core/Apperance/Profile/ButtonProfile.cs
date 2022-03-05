using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ButtonProfile), menuName = "Ui-Framework/Profile/Button", order = 0)]
public class ButtonProfile : ScriptableObject
{
    public enum ButtonProfileType
    {
        Positive,
        Negative
    }
    
    //Do Not Edit
    public enum ButtonStateType
    {
        Active,
        Inactive,
        Hover,
        Click
    }
    
    [Serializable]
    public class UiButton
    {
        public ColorPalette.ColorType colorType;

        public GenericDictionary<ButtonStateType, ButtonState> buttonStates;

        public UiButton()
        {
            buttonStates = GenericDictionary<ButtonStateType, ButtonState>.ToGenericDictionary(Utils.GetEnumValues<ButtonStateType>().ToDictionary(c => c, c => new ButtonState()));
        }
    }
    
    [Serializable]
    public class ButtonState
    {
        public ColorPalette.UiColor uiColor = ColorPalette.UiColor.DefaultUiColor;
        
        public bool isFill = true;

        public float borderWidth = 5f;
        
        public float borderRadius = 25f;
    }
    
    public GenericDictionary<ButtonProfileType, UiButton> profiles = GenericDictionary<ButtonProfileType, UiButton>
        .ToGenericDictionary(Utils.GetEnumValues<ButtonProfileType>().ToDictionary(c => c, c => new UiButton()));
}

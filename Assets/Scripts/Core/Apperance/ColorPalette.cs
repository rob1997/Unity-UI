using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ColorPalette), menuName = "Ui-Framework/Color/Palette", order = 0)]
public class ColorPalette : ScriptableObject
{
    [Serializable]
    public struct UiColor
    {
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
}
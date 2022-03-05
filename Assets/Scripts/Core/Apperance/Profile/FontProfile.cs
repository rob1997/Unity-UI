using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(FontProfile), menuName = "Ui-Framework/Profile/Font", order = 0)]
public class FontProfile : ScriptableObject
{
    public enum FontProfileType
    {
        Title,
        H1,
        H2,
        H3,
        H4,
        Display,
        BodyLarge,
        BodySmall
    }
    
    public enum FontWeightType
    {
        Black = 900,
        Heavy = 800,
        Bold = 700,
        SemiBold = 600,
        Medium = 500,
        Regular = 400,
        Light = 300,
        ExtraLight = 200,
        Thin = 100
    }
    
    
    
    [Serializable]
    public class UiFont
    {
        public float minSize = 12f;
        public float maxSize = 18f;

        public ColorPalette.ColorType fontColorType;
        public bool isOverlay = true;
        
        public FontWeightType fontWeight = FontWeightType.Regular;
    }

    public float maxFontSize = 150f;
    
    public GenericDictionary<FontProfileType, UiFont> profiles = GenericDictionary<FontProfileType, UiFont>
        .ToGenericDictionary(Utils.GetEnumValues<FontProfileType>().ToDictionary(c => c, c => new UiFont()));
}

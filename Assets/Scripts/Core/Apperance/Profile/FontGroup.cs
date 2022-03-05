using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(FontGroup), menuName = "Ui-Framework/Font/Group", order = 0)]
public class FontGroup : ScriptableObject
{
    public GenericDictionary<FontProfile.FontWeightType, TMP_FontAsset> group = GenericDictionary<FontProfile.FontWeightType, TMP_FontAsset>
        .ToGenericDictionary(Utils.GetEnumValues<FontProfile.FontWeightType>().ToDictionary(c => c, c => (TMP_FontAsset) null));
}

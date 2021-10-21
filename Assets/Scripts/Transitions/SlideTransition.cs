using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SlideTransition), menuName = "Ui-Framework/Transitions/Slide", order = 0)]
public class SlideTransition : Transition
{
    public override void Setup(MenuElement menu)
    {
        RectTransform rectT = (RectTransform) menu.transform;
        
        Vector2 initialAnchorPos = rectT.anchoredPosition;

        initialAnchorPos.x = - rectT.rect.width;
        
        rectT.anchoredPosition = initialAnchorPos;
    }
    
    public override void Load(MenuElement menu, Action onComplete = null)
    {
        RectTransform rectT = (RectTransform) menu.transform;
        
        rectT.DOKill();

        float normalizedDuration = Utils.GetNormalizedValue(Math.Abs(rectT.anchoredPosition.x), 0, rectT.rect.width);
        
        rectT.DOAnchorPosX(0, normalizedDuration * GetFullTransitionDuration()).onComplete += delegate
        {
            onComplete?.Invoke();
        };

    }
    
    public override void Unload(MenuElement menu, Action onComplete = null)
    {
        RectTransform rectT = (RectTransform) menu.transform;
        
        rectT.DOKill();
        
        float normalizedDuration = 1f - Utils.GetNormalizedValue(Math.Abs(rectT.anchoredPosition.x), 0, rectT.rect.width);

        rectT.DOAnchorPosX(- rectT.rect.width, normalizedDuration * GetFullTransitionDuration()).onComplete += delegate
        {
            Setup(menu);
            onComplete?.Invoke();
        };
    }
}

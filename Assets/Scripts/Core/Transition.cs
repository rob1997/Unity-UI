using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Transition
{
    //TODO: transition should use duration relative to target
    public float fullTransitionDuration = .5f;
    
    public void Initialize(MenuElement menu)
    {
//        menu.transform.localScale = Vector3.zero;

        Vector2 initialAnchorPos = ((RectTransform) menu.transform).anchoredPosition;

        initialAnchorPos.x -= ((RectTransform) menu.transform).rect.width;
        
        ((RectTransform) menu.transform).anchoredPosition = initialAnchorPos;
    }
    
    public void Load(MenuElement menu, Action onComplete)
    {
        menu.transform.DOKill();

        ((RectTransform) menu.transform).DOAnchorPosX(0, fullTransitionDuration).onComplete += onComplete.Invoke;
//        menu.transform.DOScale(1, duration).onComplete += onComplete.Invoke;
        
    }
    
    public void Unload(MenuElement menu, Action onComplete)
    {
        menu.transform.DOKill();
        ((RectTransform) menu.transform).DOAnchorPosX(- ((RectTransform) menu.Region.transform).rect.width, fullTransitionDuration).onComplete += delegate
        {
            Initialize(menu);
            onComplete.Invoke();
        };
//        menu.transform.DOScale(0, duration).onComplete += delegate
//        {
//            Initialize(menu);
//            onComplete.Invoke();
//        };

    }
}
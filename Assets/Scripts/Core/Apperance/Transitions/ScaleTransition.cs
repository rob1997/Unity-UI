using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ScaleTransition), menuName = "Ui-Framework/Transitions/Scale", order = 0)]
public class ScaleTransition : Transition
{
    public override void Setup(Menu menu)
    {
        Transform transform = menu.transform;
        
        transform.localScale = Vector3.zero;
    }
    
    public override void Load(Menu menu, Action onComplete = null)
    {
        Transform transform = menu.transform;
        
        transform.DOKill();

        float normalizedDuration = 1f - Utils.GetNormalizedValue(transform.localScale.magnitude, 0, 1);
        
        transform.DOScale(1, normalizedDuration * GetFullTransitionDuration()).onComplete += delegate
        {
            onComplete?.Invoke();
        };
    }
    
    public override void Unload(Menu menu, Action onComplete = null)
    {
        Transform transform = menu.transform;
        
        transform.DOKill();
        
        float normalizedDuration = Utils.GetNormalizedValue(transform.localScale.magnitude, 0, 1);
        
        transform.DOScale(0, normalizedDuration * GetFullTransitionDuration()).onComplete += delegate
        {
            Setup(menu);
            onComplete?.Invoke();
        };
    }
}

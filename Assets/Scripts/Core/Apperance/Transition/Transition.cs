using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Transition : ScriptableObject
{
    //TODO: make regions inactive (can't load/unload) during transitions
    public abstract void Setup(Menu menu);
    public abstract void Load(Menu menu, Action onComplete = null);
    public abstract void Unload(Menu menu, Action onComplete = null);

    protected float GetFullTransitionDuration()
    {
        //TODO: transition should use duration relative to target
        return UiManager.Instance.uiPreferences.defaultFullTransitionTime;
    }
}
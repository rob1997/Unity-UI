using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Expander : MonoBehaviour
{
    public RectTransform rectToExpand;
    public float duration;

    private Vector2 _initialSizeDelta;

    private void Start()
    {
        _initialSizeDelta = rectToExpand.sizeDelta;
    }

    public void Expand(float width, float height, bool open = true, bool instant = false)
    {
        rectToExpand.DOKill();
        
        Vector2 targetSizeDelta = _initialSizeDelta + new Vector2(width, height);

        if (!open) targetSizeDelta *= - 1f;

        rectToExpand.DOSizeDelta(targetSizeDelta, instant ? 0f : duration);
    }

    public void RollBack(bool instant = false)
    {
        rectToExpand.DOKill();
        
        rectToExpand.DOSizeDelta(_initialSizeDelta, instant ? 0f : duration);
    }
}

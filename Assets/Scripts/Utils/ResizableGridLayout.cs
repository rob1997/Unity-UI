using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class ResizableGridLayout : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    
    private Vector2 _initialCellSize;
    private Vector2 _initialSpacing;
    private GridLayoutGroup _gridLayoutGroup;
    private Vector2 _referenceResolution;

    private float WidthScaleFactor => Screen.width / _referenceResolution.x;
    private float HeightScaleFactor => Screen.height / _referenceResolution.y;
    
    void Start()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        
        _initialCellSize = _gridLayoutGroup.cellSize;
        _initialSpacing = _gridLayoutGroup.spacing;
        
        if (canvasScaler != null)
        {
            _referenceResolution = canvasScaler.referenceResolution;
        }

        else
        {
            _referenceResolution = new Vector2(Screen.width, Screen.height);
        }
        
        Apply();
        
        //_gridLayoutGroup.padding;
    }

#if UNITY_EDITOR
    private void Update()
    {
        Apply();
    }
#endif

    private void Apply()
    {
        _gridLayoutGroup.cellSize = new Vector2(_initialCellSize.x * WidthScaleFactor, 
            _initialCellSize.y * HeightScaleFactor);
        
        _gridLayoutGroup.spacing = new Vector2(_initialSpacing.x * WidthScaleFactor, 
            _initialSpacing.y * HeightScaleFactor);
    }
}

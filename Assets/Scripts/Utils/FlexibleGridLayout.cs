using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        FixedRows,
        FixedColumns,
    }
    
    public new RectOffset padding;
    
    public FitType fitType;
    
    public int rows;
    public int columns;
    
    public Vector2 cellSize;
    public Vector2 spacing;
    
    public bool fitX;
    public bool fitY;
    
    public bool centerX;
    public bool centerY;

    private int _childCount;
    
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        switch (fitType)
        {
            case FitType.FixedRows:
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);
                break;
            case FitType.FixedColumns:
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
                break;
        }

        Rect parentRect = rectTransform.rect;
        
        float parentWidth = parentRect.width;
        float parentHeight = parentRect.height;

        float cellWidth = (parentWidth - (spacing.x * (columns - 1)) - padding.left - padding.right) / (float) columns;
        float cellHeight = (parentHeight - (spacing.y * (rows - 1)) - padding.top - padding.bottom) / (float) rows;
        
        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int childrenCount = rectChildren.Count;
        
        for (int i = 0; i < childrenCount; i++)
        {
            int rowCount = i / columns;
            int columnCount = i % columns;

            RectTransform item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;

            if (centerX)
            {
                int itemsPerRow = rowCount + 1 == rows && childrenCount % columns != 0 ? childrenCount % columns : columns;
                
                float spaceLeftOnRow = parentWidth - (cellSize.x * itemsPerRow) - (spacing.x * (itemsPerRow - 1)) - padding.left - padding.right;
                xPos += (float) spaceLeftOnRow / 2f;
            }
            
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            if (centerY)
            {
                float spaceLeftOnColumn = parentHeight - (cellSize.y * rows) - (spacing.y * (rows - 1)) - padding.top - padding.bottom;
                yPos += (float) spaceLeftOnColumn / 2f;
            }
            
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
        
        //Set Preferred sizes
        SetLayoutInputForAxis(0, (cellSize.x * columns) + (spacing.x * (columns - 1)) + padding.left + padding.right, 0, 0);
        SetLayoutInputForAxis(0, (cellSize.y * rows) + (spacing.y * (rows - 1)) + padding.top + padding.bottom, 0, 1);
    }

    public override void CalculateLayoutInputVertical() {}
    
    public override void SetLayoutHorizontal() {}

    public override void SetLayoutVertical() {}

    private void Update()
    {
        //Whenever new element is added
        if (_childCount != transform.childCount)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            _childCount = transform.childCount;
        }
    }
}

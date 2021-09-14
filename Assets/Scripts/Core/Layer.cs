using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    [SerializeField] private UiConstants.LayerType layerType;
    
    private List<Region> _regions = new List<Region>();

    private UiRoot _uiRoot;
    
    public List<Region> Regions => _regions;
    
    public UiConstants.LayerType LayerType => layerType;
    
    public void Initialize(UiRoot uiRoot)
    {
        _uiRoot = uiRoot;
        
        _regions = new List<Region>(GetComponentsInChildren<Region>());

        foreach (Region region in _regions)
        {
            region.Initialize(this);
        }
    }

    public bool GetRegion(UiConstants.RegionType regionType, out Region region)
    {
        region = _regions.Find(r => r.RegionType == regionType);

        return region != null;
    }
    
    public bool HasRegion(UiConstants.RegionType regionType)
    {
        return _regions.Find(r => r.RegionType == regionType) != null;
    }
}

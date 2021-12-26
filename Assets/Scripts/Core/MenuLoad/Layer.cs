using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Layer : MonoBehaviour
{
    [FormerlySerializedAs("layerType")] [SerializeField] private UiConstants.UiLayer uiLayer;
    
    private List<Region> _regions = new List<Region>();

    private UiRoot _uiRoot;
    
    public List<Region> Regions => _regions;
    
    public UiConstants.UiLayer UiLayer => uiLayer;
    
    public void Initialize(UiRoot uiRoot)
    {
        _uiRoot = uiRoot;
        
        _regions = new List<Region>(GetComponentsInChildren<Region>());

        foreach (Region region in _regions)
        {
            region.Initialize(this);
        }
    }

    public bool GetRegion(UiConstants.UiRegion uiRegion, out Region region)
    {
        region = _regions.Find(r => r.UiRegion == uiRegion);

        return region != null;
    }
    
    public bool HasRegion(UiConstants.UiRegion uiRegion)
    {
        return _regions.Find(r => r.UiRegion == uiRegion) != null;
    }
}

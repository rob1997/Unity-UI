using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UiRoot : MonoBehaviour
{
    public Transform layerRoot;
    
    private List<Layer> _layers;

    public List<Layer> Layers => _layers;
    
    public void Initialize()
    {
        if (layerRoot == null) layerRoot = transform;
        
        _layers = new List<Layer>(layerRoot.GetComponentsInChildren<Layer>());

        foreach (Layer layer in _layers)
        {
            layer.Initialize(this);
        }

        foreach (UiConstants.MenuType menuType in UiConstants.StartingMenus)
        {
            LoadMenu(menuType);
        }
    }

    public void LoadMenu(UiConstants.MenuType menuType)
    {
        if (UiManager.Instance.GetMenuReference(menuType, out AssetReference menuRef))
        {
            Utils.LoadAsset<GameObject>(menuRef.AssetGUID, obj =>
            {
                GameObject menuPrefab = obj;

                if (menuPrefab.TryGetComponent(out MenuElement menu))
                {
                    if (GetRegion(menu.LayerType, menu.RegionType, out Region region))
                    {
                        region.LoadMenu(menuPrefab);
                    }

                    else
                    {
                        Debug.LogError($"can't find {menu.RegionType} region on {menu.LayerType}");
                    }
                }
                    
                else
                {
                    Debug.LogError($"can't find Menu on {menuPrefab}");
                }
            });
        }
        
        else
        {
            Debug.LogError($"Can't find address for {menuType} menu");
        }
    }

    public bool HasLayer(UiConstants.LayerType layerType)
    {
        return _layers.Find(l => l.LayerType == layerType) != null;
    }
    
    public bool GetLayer(UiConstants.LayerType layerType, out Layer layer)
    {
        layer = _layers.Find(l => l.LayerType == layerType);
        
        return layer != null;
    }
    
    public bool GetRegion(UiConstants.LayerType layerType, UiConstants.RegionType regionType, out Region region)
    {
        region = null;
        
        if (GetLayer(layerType, out Layer layer) && layer.GetRegion(regionType, out region))
        {
            return region != null;
        }

        else
        {
            return false;
        }
    }
}

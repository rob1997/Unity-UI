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
    
    public ModalFactory ModalFactory { get; } = new ModalFactory();
    
    public void Initialize()
    {
        if (layerRoot == null) layerRoot = transform;
        
        _layers = new List<Layer>(layerRoot.GetComponentsInChildren<Layer>());

        foreach (Layer layer in _layers)
        {
            layer.Initialize(this);
        }

        foreach (UiConstants.UiMenu menuType in UiManager.Instance.uiPreferences.startingMenus)
        {
            LoadMenu(menuType);
        }
    }

    public void LoadMenu(UiConstants.UiMenu uiMenu)
    {
        if (UiManager.Instance.GetMenuReference(uiMenu, out AssetReference menuRef))
        {
            Utils.LoadAsset<GameObject>(menuRef.AssetGUID, obj =>
            {
                GameObject menuPrefab = obj;

                if (menuPrefab.TryGetComponent(out Menu menu))
                {
                    if (GetRegion(menu.UiLayer, menu.UiRegion, out Region region))
                    {
                        region.LoadMenu(menuPrefab);
                    }

                    else Debug.LogError($"can't find {menu.UiRegion} region on {menu.UiLayer}");
                }
                    
                else Debug.LogError($"can't find Menu on {menuPrefab}");
            });
        }
        
        else Debug.LogError($"Can't find address for {uiMenu} menu");
    }

    public bool HasLayer(UiConstants.UiLayer uiLayer)
    {
        return _layers.Find(l => l.UiLayer == uiLayer) != null;
    }
    
    public bool GetLayer(UiConstants.UiLayer uiLayer, out Layer layer)
    {
        layer = _layers.Find(l => l.UiLayer == uiLayer);
        
        return layer != null;
    }
    
    public bool GetRegion(UiConstants.UiLayer uiLayer, UiConstants.UiRegion uiRegion, out Region region)
    {
        region = null;
        
        if (GetLayer(uiLayer, out Layer layer) && layer.GetRegion(uiRegion, out region))
        {
            return region != null;
        }

        else return false;
    }
}

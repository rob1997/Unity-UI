using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UiManager : Singleton<UiManager>
{
    public AssetReference uIRootObjectReference;

    [SerializeField] private GenericDictionary<UiConstants.MenuType, AssetReference> menuReferences;

    private UiRoot _uiRoot;
    
    public UiRoot UiRoot => _uiRoot;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Utils.LoadAsset<GameObject>(uIRootObjectReference.AssetGUID, obj =>
        {
            GameObject uiRootObj = Instantiate(obj);

            if (uiRootObj.TryGetComponent(out _uiRoot))
            {
                _uiRoot.Initialize();
            }
            
            else
            {
                Debug.LogError($"{obj} not of type {typeof(UiRoot)}");
            }
        });
    }
    
    public bool GetMenuReference(UiConstants.MenuType menuType, out AssetReference menuReference)
    {
        return menuReferences.TryGetValue(menuType, out menuReference);
    }
}

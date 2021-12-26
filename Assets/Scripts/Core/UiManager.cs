using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UiManager : Singleton<UiManager>
{
    public AssetReference uIRootObjectReference;
    
    public UiPreferences uiPreferences;

    [HideInInspector] public GenericDictionary<UiConstants.UiMenu, AssetReference> menuReferences;

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
    
    public bool GetMenuReference(UiConstants.UiMenu uiMenu, out AssetReference menuReference)
    {
        return menuReferences.TryGetValue(uiMenu, out menuReference);
    }
}
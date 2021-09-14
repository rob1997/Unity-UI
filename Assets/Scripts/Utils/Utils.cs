using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Utils
{
    public static void LoadAsset<T>(string address, Action<T> onLoad)
    {
        Addressables.LoadAssetAsync<T>(address).Completed += handle =>
        {
            if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Can't load asset at {address}");
                
                return;
            }
            
            onLoad.Invoke(handle.Result);
        };
    }
}

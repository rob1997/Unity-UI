using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    
    public static T[] GetEnumValues<T>() where T : struct 
    {
        if (!typeof(T).IsEnum) 
        {
            throw new ArgumentException("GetValues<T> can only be called for types derived from System.Enum", "T");
        }
        
        return (T[])Enum.GetValues(typeof(T));
    }

    public static string GetDisplayName(string name)
    {
        string displayName = Regex.Replace($"{name}", "(\\B[A-Z])", " $1");

        //make first char upper case
        return string.Concat(displayName[0].ToString().ToUpper(), displayName.Substring(1));
    }
    
    public static float GetNormalizedValue(float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);

        return (value - min) / (max - min);
    }
}

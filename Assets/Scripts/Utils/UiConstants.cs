using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UiConstants
{
    public enum LayerType
    {
        Main,
        Window
    }
    
    public enum RegionType
    {
        Main,
        TopBar,
        BottomBar,
        LeftBar
    }
    
    /// <summary>
    /// IMPORTANT - ALWAYS add new MenuType to the bottom of enum or <see cref="UiManager.menuReferences"/> serialization will be reset
    /// </summary>
    public enum MenuType
    {
        Home,
        TopBar,
        BottomBar,
        Window,
        Settings,
        
    }

    public enum UnloadMode
    {
        Remove,
        Cache,
    }

    public static readonly float CacheTimeOutSeconds = 5f;
    
    public static List<MenuType> StartingMenus = new List<MenuType>
    {
        MenuType.Home,
        MenuType.TopBar,
        MenuType.BottomBar,
    };
}

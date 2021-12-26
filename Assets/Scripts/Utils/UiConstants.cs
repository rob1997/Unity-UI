using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public static class UiConstants
{
    #region MENU

    public enum UiLayer
    {
        Main,
        Window
    }
    
    public enum UiRegion
    {
        Main,
        TopBar,
        BottomBar,
        LeftBar
    }
    
    /// <summary>
    /// IMPORTANT - ALWAYS add new MenuType to the bottom of enum or <see cref="UiManager.menuReferences"/> serialization will be reset
    /// </summary>
    public enum UiMenu
    {
        Home,
        TopBar,
        BottomBar,
        Window,
        Settings
    }

    public enum UnloadMode
    {
        //destroy
        Remove,
        //disable
        Cache,
    }

    #endregion
}

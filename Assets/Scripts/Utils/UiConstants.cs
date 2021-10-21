using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class UiConstants
{
    #region MENU

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
        //destroy
        Remove,
        //disable
        Cache,
    }

    public static readonly float CacheTimeOutSeconds = 5f;
    
    public static List<MenuType> StartingMenus = new List<MenuType>
    {
        MenuType.Home,
        MenuType.TopBar,
        MenuType.BottomBar,
    };

    #endregion

    #region Transition

    public static readonly TransitionType DefaultTransition = TransitionType.HSlide;
    public static readonly float DefaultFullTransitionTime = .5f;
    
    public enum TransitionType
    {
        HSlide,
        Scale
    }

    public static readonly Dictionary<TransitionType, Transition> Transitions = new Dictionary<TransitionType, Transition>
    {
        {TransitionType.HSlide, ScriptableObject.CreateInstance<SlideTransition>()},
        {TransitionType.Scale, ScriptableObject.CreateInstance<ScaleTransition>()},
    };
    
    #endregion
}

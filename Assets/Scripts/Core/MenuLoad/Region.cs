using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class Region : MonoBehaviour
{
    [FormerlySerializedAs("regionType")] [SerializeField] private UiConstants.UiRegion uiRegion;
    [SerializeField] private Transition transition;

    private List<Menu> _menus = new List<Menu>();

    private Layer _layer;

    public UiConstants.UiRegion UiRegion => uiRegion;
    
    public List<Menu> Menus => _menus;

    public Layer Layer => _layer;
    
    public Transition Transition
    {
        get
        {
            if (transition == null)
            {
                transition = UiManager.Instance.uiPreferences.defaultTransition;
            }

            return transition;
        }
    }

    public void Initialize(Layer layer)
    {
        _layer = layer;
    }

    public Menu GetActiveMenu()
    {
        return _menus.Find(m => m.IsActive);
    }
    
    public bool HasMenu(UiConstants.UiMenu uiMenu)
    {
        return _menus.Find(m => m.UiMenu == uiMenu) != null;
    }
    
    public bool GetMenu(UiConstants.UiMenu uiMenu, out Menu menu)
    {
        menu = _menus.Find(m => m.UiMenu == uiMenu);

        return menu != null;
    }

    public void UnloadMenu(Menu menu)
    {
        if (menu.IsActive) menu.Deactivate();

        else Debug.LogError($"Can't Unload, {menu.UiMenu} menu already inactive");
    }
    
    public void Activate(Menu menu)
    {
        if (menu.IsActive)
        {
            Debug.LogError($"Can't Activate, {menu.UiMenu} menu already active");
            
            return;
        }

        Menu activeMenu = GetActiveMenu();

        if (activeMenu != null)
        {
            UnloadMenu(activeMenu);
        }
        
        menu.Activate();
    }

    public void Activate(UiConstants.UiMenu uiMenu)
    {
        if (GetMenu(uiMenu, out Menu menu))
        {
            Activate(menu);
        }
        
        else
        {
            Debug.LogError($"Can't activate {uiMenu} menu, menu not found");
        }
    }
    
    
    public void LoadMenu(UiConstants.UiMenu uiMenu)
    {
        if (GetMenu(uiMenu, out Menu menu))
        {
            Activate(menu);
        }

        else
        {
            if (UiManager.Instance.GetMenuReference(uiMenu, out AssetReference menuRef))
            {
                Utils.LoadAsset<GameObject>(menuRef.AssetGUID, LoadMenu);
            }

            else
            {
                Debug.LogError($"Can't find address for {uiMenu} menu");
            }
        }
    }

    public void LoadMenu(GameObject menuPrefab)
    {
        //Add then activate added Menu
        Activate(AddMenu(menuPrefab));
    }
    
    public Menu AddMenu(GameObject menuPrefab)
    {
        if (menuPrefab.TryGetComponent(out Menu prefabMenu))
        {
            UiConstants.UiMenu uiMenu = prefabMenu.UiMenu;
            
            if (GetMenu(uiMenu, out Menu menu))
            {
                //Debug.LogError($"{menuType} Menu already exists in {regionType} region");
                
                return menu;
            }
            
            GameObject menuObj = Instantiate(menuPrefab, transform);

            menu = menuObj.GetComponent<Menu>();
            
            _menus.Add(menu);

            menu.Initialize(this);

            return menu;
        }
        
        else
        {
            Debug.LogError($"{menuPrefab} not a Menu type");

            return null;
        }
    }
}

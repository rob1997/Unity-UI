using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Region : MonoBehaviour
{
    public Transition transition = new Transition();
    
    [SerializeField] private UiConstants.RegionType regionType;

    private List<MenuElement> _menus = new List<MenuElement>();

    private Layer _layer;

    public UiConstants.RegionType RegionType => regionType;
    
    public List<MenuElement> Menus => _menus;

    public Layer Layer => _layer;

    public void Initialize(Layer layer)
    {
        _layer = layer;
    }

    public MenuElement GetActiveMenu()
    {
        return _menus.Find(m => m.IsActive);
    }
    
    public bool HasMenu(UiConstants.MenuType menuType)
    {
        return _menus.Find(m => m.MenuType == menuType) != null;
    }
    
    public bool GetMenu(UiConstants.MenuType menuType, out MenuElement menu)
    {
        menu = _menus.Find(m => m.MenuType == menuType);

        return menu != null;
    }

    public void UnloadMenu(MenuElement menu)
    {
        if (menu.IsActive) menu.Deactivate();
        
        transition.Unload(menu, delegate
        {
            switch (menu.UnloadMode)
            {
                case UiConstants.UnloadMode.Remove:
                    menu.Remove();
                    break;
                case UiConstants.UnloadMode.Cache:
                    menu.Cache();
                    break;
            }
        });
    }
    
    public void Activate(MenuElement menu)
    {
        if (menu.IsActive)
        {
            Debug.LogError($"{menu.MenuType} menu already active");
            
            return;
        }

        MenuElement activeMenu = GetActiveMenu();

        if (activeMenu != null)
        {
            activeMenu.Deactivate();

            UnloadMenu(activeMenu);
            
//            UnloadMenu(activeMenu);
        }
        
        menu.Activate();

        transition.Load(menu, delegate {  });
    }

    public void Activate(UiConstants.MenuType menuType)
    {
        if (GetMenu(menuType, out MenuElement menu))
        {
            Activate(menu);
        }
        
        else
        {
            Debug.LogError($"Can't activate {menuType} menu, menu not found");
        }
    }
    
    
    public void LoadMenu(UiConstants.MenuType menuType)
    {
        if (GetMenu(menuType, out MenuElement menu))
        {
            Activate(menu);
        }

        else
        {
            if (UiManager.Instance.GetMenuReference(menuType, out AssetReference menuRef))
            {
                Utils.LoadAsset<GameObject>(menuRef.AssetGUID, LoadMenu);
            }

            else
            {
                Debug.LogError($"Can't find address for {menuType} menu");
            }
        }
    }

    public void LoadMenu(GameObject menuPrefab)
    {
        //Add then activate added Menu
        Activate(AddMenu(menuPrefab));
    }
    
    public MenuElement AddMenu(GameObject menuPrefab)
    {
        if (menuPrefab.TryGetComponent(out MenuElement prefabMenu))
        {
            UiConstants.MenuType menuType = prefabMenu.MenuType;
            
            if (GetMenu(menuType, out MenuElement menu))
            {
                //Debug.LogError($"{menuType} Menu already exists in {regionType} region");
                
                return menu;
            }
            
            GameObject menuObj = Instantiate(menuPrefab, transform);

            menu = menuObj.GetComponent<MenuElement>();
            
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

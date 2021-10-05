using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuElement : UiElement
{
    [SerializeField] private UiConstants.LayerType layerType;
    [SerializeField] private UiConstants.RegionType regionType;
    [SerializeField] private UiConstants.MenuType menuType;
    [SerializeField] private UiConstants.UnloadMode unloadMode;

    private Region _region;
    
    /// <summary>
    /// coroutine that destroys cached menu after <see cref="UiConstants.CacheTimeOutSeconds"/> of being cached
    /// </summary>
    private Coroutine _destroyCacheCoroutine;

    public Region Region => _region;
    
    public Layer Layer => _region.Layer;
    
    public UiConstants.LayerType LayerType => layerType;
    public UiConstants.RegionType RegionType => regionType;
    public UiConstants.MenuType MenuType => menuType;
    public UiConstants.UnloadMode UnloadMode => unloadMode;
    
    public bool IsActive { get; private set; }

    public void Initialize(Region region)
    {
        _region = region;
        
        _region.transition.Initialize(this);
    }
    
    public void Activate()
    {
        IsActive = true;
        
        gameObject.SetActive(true);

        if (_destroyCacheCoroutine != null) _region.StopCoroutine(_destroyCacheCoroutine);
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }

    public void Remove()
    {
        if (IsActive) Deactivate();

        _region.Menus.Remove(this);

        Destroy(gameObject);
    }
    
    public void Cache()
    {
        if (IsActive) Deactivate();
        
        gameObject.SetActive(false);
        
        _destroyCacheCoroutine = _region.StartCoroutine(WaitThenRemove());
    }

    IEnumerator WaitThenRemove()
    {
        yield return new WaitForSeconds(UiConstants.CacheTimeOutSeconds);
        
        Remove();
    }
}
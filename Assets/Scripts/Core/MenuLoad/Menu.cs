using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Menu : UiElement
{
    #region Activated

    public delegate void Activated();

    public event Activated OnActivated;

    private void InvokeActivated()
    {
        OnActivated?.Invoke();
    }

    #endregion

    #region Deactivated

    public delegate void Deactivated();

    public event Deactivated OnDeactivated;

    private void InvokeDeactivated()
    {
        OnDeactivated?.Invoke();
    }

    #endregion

    //Invoked when menu is already activated and completed load transition
    #region Loaded

    public delegate void Loaded();

    public event Loaded OnLoaded;

    private void InvokeLoaded()
    {
        OnLoaded?.Invoke();
    }

    #endregion

    //Invoked when menu is already deactivated and completed unload transition
    #region Unloaded

    public delegate void Unloaded();

    public event Unloaded OnUnloaded;

    private void InvokeUnloaded()
    {
        OnUnloaded?.Invoke();
    }

    #endregion
    
    [FormerlySerializedAs("layerType")] [SerializeField] private UiConstants.UiLayer uiLayer;
    [FormerlySerializedAs("regionType")] [SerializeField] private UiConstants.UiRegion uiRegion;
    [FormerlySerializedAs("menuType")] [SerializeField] private UiConstants.UiMenu uiMenu;
    [SerializeField] private UiConstants.UnloadMode unloadMode;

    private Region _region;
    
    /// <summary>
    /// coroutine that destroys cached menu after <see cref="UiPreferences.cacheTimeOutSeconds"/> of being cached
    /// </summary>
    private Coroutine _destroyCacheCoroutine;

    public Region Region => _region;
    
    public Layer Layer => _region.Layer;
    
    public UiConstants.UiLayer UiLayer => _region != null ? _region.Layer.UiLayer : uiLayer;
    public UiConstants.UiRegion UiRegion => _region != null ? _region.UiRegion : uiRegion;
    public UiConstants.UiMenu UiMenu => uiMenu;
    public UiConstants.UnloadMode UnloadMode => unloadMode;
    
    public bool IsActive { get; private set; }
    
    public Transition Transition => _region.Transition;
    
    public virtual void Initialize(Region region)
    {
        _region = region;

        uiRegion = UiRegion;
        uiLayer = UiLayer;
        
        Transition.Setup(this);
    }
    
    public void Activate()
    {
        IsActive = true;
        
        gameObject.SetActive(true);

        gameObject.transform.SetAsLastSibling();
        
        if (_destroyCacheCoroutine != null) _region.StopCoroutine(_destroyCacheCoroutine);
        
        if (Transition != null) Transition.Load(this, InvokeLoaded);
        
        InvokeActivated();
    }
    
    public void Deactivate()
    {
        IsActive = false;

        if (Transition != null) Transition.Unload(this, Unload);

        else Unload();
        
        InvokeDeactivated();
    }

    private void Unload()
    {
        switch (UnloadMode)
        {
            case UiConstants.UnloadMode.Remove:
                Remove();
                break;
            case UiConstants.UnloadMode.Cache:
                Cache();
                break;
        }
        
        InvokeUnloaded();
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
        yield return new WaitForSeconds(UiManager.Instance.uiPreferences.cacheTimeOutSeconds);
        
        Remove();
    }
}
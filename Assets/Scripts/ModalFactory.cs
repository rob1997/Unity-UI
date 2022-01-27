using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UiModal
{
    Info = 0,
    Warning = 1,
    Success = 2,
    Error = 3
}

public struct ModalAction
{
    public string Title { get; }

    public Action Action { get; }

    public ModalAction(string title, Action action)
    {
        Title = title;
        Action = action;
    }
}

public class Modal
{
    public string Message { get; private set; }
    public UiModal ModalType { get; private set; }
    public float Timeout { get; private set; }
    public DateTime TimeStamp { get; private set; }
    public ModalAction[] Actions { get; private set; }
    /// <summary>
    /// Can't close Modal using overlay, must pick an Action (option) to opt out
    /// </summary>
    public bool IsWall { get; private set; }
    public Action OnUnloaded { get; private set; }
    
    public Modal(string message, UiModal modalType = UiModal.Info, float timeout = 0, ModalAction[] actions = null, bool isWall = false, Action onUnloaded = null)
    {
        Message = message;
        ModalType = modalType;
        Timeout = timeout;
        TimeStamp = DateTime.Now;
        Actions = actions;
        //There need to be actions for Modal to be a wall
        if (Actions == null && isWall) Debug.LogWarning("Modal needs actions to have a wall");
        IsWall = Actions != null && isWall;
        OnUnloaded = onUnloaded;
    }
}

public class ModalFactory
{
    public int QueuedCount => _modals.Count;

    private ModalMenu _modalMenu;
    
    private readonly Queue<Modal> _modals = new Queue<Modal>();
    
    public void CreateModal(Modal modal)
    {
        _modals.Enqueue(modal);

        if (_modals.Count > 1 || (_modalMenu != null && _modalMenu.IsActive)) return;
        
        //Load if Modal Menu isn't loaded else it'll handled concurrently
        UiManager.Instance.UiRoot.LoadMenu(UiConstants.UiMenu.Modal);
    }

    public void SetModalMenu(ModalMenu modalMenu) => _modalMenu = modalMenu;

    public Modal GetModal()
    {
        if (_modals.Count < 0) return null;
        
        return _modals.Dequeue();
    }
}

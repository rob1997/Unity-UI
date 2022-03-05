using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ModalMenu : Menu
{
    [SerializeField] private Button overlayButton;
    [SerializeField] private Expander expander;
    [SerializeField] private TextMeshProUGUI modalMessageText;
    [SerializeField] private TextMeshProUGUI timeStampText;
    [Space]
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AssetReference buttonAddress;
    [Space] 
    [SerializeField] private Transform modalIconsContainer;

    private ModalFactory _modalFactory;
    private Coroutine _waitThenClose;
    private Action _onModalUnloaded;

    public override void Initialize(Region region)
    {
        base.Initialize(region);

        _modalFactory = UiManager.Instance.UiRoot.ModalFactory;
        _modalFactory.SetModalMenu(this);
        
        OnUnloaded += OnModalUnloaded;
        
        OnActivated += OnModalActivated;
        
        overlayButton.onClick.AddListener(CloseModal);
    }
    
    private void AttachModal(Modal modal)
    {
        modalMessageText.text = modal.Message;

        //Set corresponding Modal type
        modalIconsContainer.GetChild((int) modal.ModalType).gameObject.SetActive(true);

        //If modal has actions
        if (modal.Actions != null && modal.Actions.Length > 0)
        {
            //Instantiate Button for each action
            Utils.LoadAsset<GameObject>(buttonAddress.AssetGUID, result =>
            {
                foreach (ModalAction action in modal.Actions)
                {
                    GameObject buttonObj = Instantiate(result, buttonContainer);
                    
                    if (buttonObj.TryGetComponent(out ButtonProfiler buttonProfiler))
                    {
                        buttonProfiler.Initialize(action.Title, delegate
                        {
                            action.Action.Invoke();
                        
                            CloseModal();
                        }, action.ButtonProfileType);
                    }

                    else
                    {
                        Debug.LogWarning("ButtonProfiler Component not found on Button");
                        
                        buttonObj.GetComponent<Button>().onClick.AddListener(delegate
                        {
                            action.Action.Invoke();
                        
                            CloseModal();
                        });

                        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                        
                        if (buttonText != null) buttonText.text = action.Title;

                        else Debug.LogWarning("No Text Component Found on Button");
                    }
                }
            });

            if (modal.IsWall) overlayButton.interactable = false;
        }
        
        if (modal.Timeout > 0) _waitThenClose = StartCoroutine(WaitThenClose(modal.Timeout));
        
        timeStampText.text = modal.TimeStamp.ToString("h:mm:ss tt");

        if (modal.OnUnloaded != null) _onModalUnloaded = modal.OnUnloaded;
    }

    private void OnModalActivated()
    {
        //stop timer coroutine if it's still running
        if (_waitThenClose != null) StopCoroutine(_waitThenClose);
            
        AttachModal(_modalFactory.GetModal());
            
        //Expand message box based on text
        StartCoroutine(InitializeExpander());
    }
    
    private void OnModalUnloaded()
    {
        //Prepare for activation if menu isn't destroyed
        if (UnloadMode == UiConstants.UnloadMode.Cache)
        {
            //Destroy all buttons
            for (int i = 0; i < buttonContainer.childCount; i++) Destroy(buttonContainer.GetChild(i).gameObject);

            //Disable all modal icons
            for (int i = 0; i < modalIconsContainer.childCount; i++) modalIconsContainer.GetChild(i).gameObject.SetActive(false);
            
            overlayButton.interactable = true;
        }

        if (_onModalUnloaded != null)
        {
            _onModalUnloaded.Invoke();

            _onModalUnloaded = null;
        }
        
        //Reload Menu if there's a Queued Modal
        if (_modalFactory.QueuedCount > 0) Region.LoadMenu(UiMenu);
    }
    
    private IEnumerator WaitThenClose(float secs)
    {
        yield return new WaitForSeconds(secs);
        
        CloseModal();
    }               
    
    private IEnumerator InitializeExpander()
    {
        yield return new WaitForEndOfFrame();

        if (modalMessageText.isTextTruncated)
        {
            expander.Expand(0, modalMessageText.preferredHeight - modalMessageText.renderedHeight, instant: true);
        }
    }

    private void CloseModal()
    {
        Region.UnloadMenu(this);
    }
}

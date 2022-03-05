using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarMenu : Menu
{
    [Space] [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button profileButton;
    
    void Start()
    {
        settingsButton.onClick.AddListener(delegate { UiManager.Instance.UiRoot.LoadMenu(UiConstants.UiMenu.Settings); });
        profileButton.onClick.AddListener(delegate
        {
            UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Modal Test"));
            
            UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("[DONT CLOSE] Queued Timeout 5 Secs Modal Test", UiModal.Warning, 5f, onUnloaded: delegate
            {
                UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("On Close Modal Test", UiModal.Error, isWall: true));
            }));
            
            UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test Long Modal Test"));
            UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Actions Test 1", isWall: true, actions: new ModalAction[]
            {
                new ModalAction("Option 1", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 1")); }, ButtonProfile.ButtonProfileType.Negative), 
                new ModalAction("Option 2", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 2")); }), 
                new ModalAction("Option 3", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 3")); }, ButtonProfile.ButtonProfileType.Negative), 
                new ModalAction("Option 4", delegate 
                { 
                    UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 4", UiModal.Success, actions: new ModalAction[]
                    {
                        new ModalAction("Option 2 1", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 2 1")); }, ButtonProfile.ButtonProfileType.Negative), 
                        new ModalAction("Option 2 2", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 2 2")); }), 
                        new ModalAction("Option 2 3", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 2 3")); }, ButtonProfile.ButtonProfileType.Negative),
                        new ModalAction("Option 2 4", delegate 
                        { 
                            UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 2 4", isWall: true, actions: new ModalAction[]
                            {
                                new ModalAction("Option 3 1", delegate { UiManager.Instance.UiRoot.ModalFactory.CreateModal(new Modal("Option 3 1")); }), 
                            })); 
                        }),
                        
                    })); 
                }),
            }));
        });
    }
}

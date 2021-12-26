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
        profileButton.onClick.AddListener(delegate { UiManager.Instance.UiRoot.LoadMenu(UiConstants.UiMenu.Window); });
    }
}

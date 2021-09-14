using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarMenu : MenuElement
{
    [Space] [Header("Buttons")]
    [SerializeField] private Button homeButton;
    
    void Start()
    {
        homeButton.onClick.AddListener(delegate { UiManager.Instance.UiRoot.LoadMenu(UiConstants.MenuType.Home); });
    }
}
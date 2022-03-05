using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

[ExecuteInEditMode]
[RequireComponent(typeof(Button))]
public class ButtonProfiler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public ButtonProfile.ButtonProfileType buttonProfileType;
    [HideInInspector] public ButtonProfile buttonProfile;
    
    private Button _button;
    private bool _isActive;
    private bool _isPressInBounds;
    private ButtonProfile.ButtonStateType _state;
    
    private bool IsActive => _button.IsActive() && _button.IsInteractable();

    private ButtonProfile.ButtonStateType State
    {
        get => _state;

        set
        {
            if (_state != value)
            {
                _state = value;
                Apply();
            }
        }
    }

    private ButtonProfile.UiButton UiButton => buttonProfile.profiles[buttonProfileType];

    private void Awake()
    {
        _button = GetComponent<Button>();

        //Reset color block
        _button.colors = new ColorBlock
        {
            normalColor = Color.white,
            highlightedColor = Color.white,
            pressedColor = Color.white,
            selectedColor = Color.white,
            disabledColor = Color.white,
            colorMultiplier    = 1.0f,
            fadeDuration       = 0.1f
        };
        
        _isActive = IsActive;

        _state = IsActive ? ButtonProfile.ButtonStateType.Active : ButtonProfile.ButtonStateType.Inactive;
    }

    public void Initialize(string text, Action action, ButtonProfile.ButtonProfileType profileType)
    {
        _button.onClick.AddListener(action.Invoke);

        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (buttonText != null) buttonText.text = text;

        else Debug.LogWarning("No Text Component Found on Button");

        buttonProfileType = profileType;
    }
    
    private void OnEnable()
    {
        if (buttonProfile == null)
        {
            try
            {
#if UNITY_EDITOR
                buttonProfile = AssetDatabase.LoadAssetAtPath<UiPreferences>(UiPreferences.DefaultUiPreferencePath)
                    .defaultButtonProfile;
#endif
                if (UiManager.Instance != null) buttonProfile = UiManager.Instance.uiPreferences.defaultButtonProfile;
            }
            
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        Apply();
    }
    
    private void Update()
    {
        if (_isActive != IsActive)
        {
            _isActive = IsActive;
            State = IsActive ? ButtonProfile.ButtonStateType.Active : ButtonProfile.ButtonStateType.Inactive;
        }
        
#if UNITY_EDITOR
        Apply();
#endif
    }
    
    public void Apply()
    {
        if (_button == null) _button = GetComponent<Button>();
        
        Graphic targetGraphic = _button.targetGraphic;

        if (targetGraphic != null)
        {
            //Apply Target Graphic color
            ButtonProfile.ButtonState buttonState = UiButton.buttonStates[State];
            
            ColorPalette.UiColor buttonStateColor = buttonState.uiColor;
            
            if (ColorPalette.GetUiColor(UiButton.colorType, out ColorPalette.UiColor buttonColor))
            {
                Color targetColor = buttonColor.main * buttonStateColor.main;
                targetGraphic.color = targetColor;
            }
            
            else Debug.LogError("Error fetching color from Palette");

            if (targetGraphic is ProceduralImage targetProceduralImage)
            {
                targetProceduralImage.ModifierType = typeof(UniformModifier);
                targetProceduralImage.GetComponent<UniformModifier>().Radius = buttonState.borderRadius;

                targetProceduralImage.BorderWidth = buttonState.isFill ? 0f : buttonState.borderWidth;
            }
            
            //Apply Overlay Color
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out Graphic overlayGraphic))
                {
                    Color targetOverlayColor = buttonColor.overlay * buttonStateColor.overlay;

                    overlayGraphic.color = targetOverlayColor;
                    
                    break;
                }
            }
        }

        else
        {
            Debug.LogError("Button has no Target Graphic");
        }
    }
    
    public void OnPointerDown(PointerEventData _)
    {
        State =  IsActive ? ButtonProfile.ButtonStateType.Click : ButtonProfile.ButtonStateType.Inactive;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        State =  IsActive ? 
            (_isPressInBounds ? ButtonProfile.ButtonStateType.Hover : ButtonProfile.ButtonStateType.Active) 
            : ButtonProfile.ButtonStateType.Inactive;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        _isPressInBounds = true;
        
        State =  IsActive ? ButtonProfile.ButtonStateType.Hover : ButtonProfile.ButtonStateType.Inactive;
    }

    public void OnPointerExit(PointerEventData _)
    {
        _isPressInBounds = false;
        
        State =  IsActive ? ButtonProfile.ButtonStateType.Active : ButtonProfile.ButtonStateType.Inactive;
    }
}

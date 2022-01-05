using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

/// <summary>
/// use PanelSwitch instead, is supports all UISwitch effects
/// </summary>
public class PanelButton : MonoBehaviour, IPointerClickHandler
{
    public enum PanelButtonAction { Activate, Deactivate, Toggle }

    public Panel panel;
    public PanelButtonAction action;

    public GameObject OnIndicator, OffIndicator;

    public MaskableGraphic[] coloredElements;
    public Color OnColor;
    public Color OffColor;

    void OnEnable()
    {
        panel.Activated += UpdateElements;
        panel.Deactivated += UpdateElements;
        UpdateElements();
    }

    void OnDisable()
    {
        panel.Activated -= UpdateElements;
        panel.Deactivated -= UpdateElements;
    }

    public void UpdateElements()
    {
        if (OnIndicator != null) OnIndicator.SetActive(panel.isActive);
        if (OffIndicator != null) OffIndicator.SetActive(!panel.isActive);
        for (int i = 0; i < coloredElements.Length; i++)
        {
            coloredElements[i].color = panel.isActive ? OnColor : OffColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (action)
        {
            case PanelButtonAction.Activate:
                panel.Activate();
                break;
            case PanelButtonAction.Deactivate:
                panel.Deactivate();
                break;
            case PanelButtonAction.Toggle:
                panel.Toggle();
                break;
            default:
                break;
        }
    }
}

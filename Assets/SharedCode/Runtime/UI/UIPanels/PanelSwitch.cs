using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitch : UISwitch
{
    public enum PanelButtonAction { Activate, Deactivate, Toggle }

    public Panel panel;
    public PanelButtonAction action;
    void OnEnable()
    {
        Init();
        Set(panel.isActive);
        panel.Activated += OnPanelStateChanged;
        panel.Deactivated += OnPanelStateChanged; 
    }

    void OnDisable()
    {
        if (group != null) group.RemoveSwitch(this);
        panel.Activated -= OnPanelStateChanged;
        panel.Deactivated -= OnPanelStateChanged;
    }

    public void OnPanelStateChanged()
    {
        Set(panel.isActive);
    }
    
    public override void OnClick()
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

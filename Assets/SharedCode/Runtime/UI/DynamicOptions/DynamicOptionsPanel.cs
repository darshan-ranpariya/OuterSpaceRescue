using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOptionsPanel : MonoBehaviour
{
    public Panel panel;
    public DynamicOptions selection;
    public string matchKey;
    public int matchIndex;

    void OnEnable()
    {
        if (panel == null) panel = GetComponent<Panel>(); 
        selection.OptionsUpdated += OnOptionsUpdated; 
        selection.SelectionChanged += OnSelectionChanged;
    }

    void OnDisable()
    { 
        selection.OptionsUpdated -= OnOptionsUpdated; 
        selection.SelectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        UpdateStatus();
    }

    private void OnOptionsUpdated()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        if (string.IsNullOrEmpty(matchKey))
        {
            if (selection.selectedIndex == matchIndex) panel.Activate();
            else panel.Deactivate();
        }
        else
        {
            if (selection.selectedOption.key.Contains(matchKey)) panel.Activate();
            else panel.Deactivate();
        }
    }
}

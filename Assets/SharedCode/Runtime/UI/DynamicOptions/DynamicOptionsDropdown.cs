using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DynamicOptionsDropdown : MonoBehaviour
{
    public DynamicOptions selection;
    Dropdown dd;

    void OnEnable()
    {
        if(dd==null) dd = GetComponent<Dropdown>();
        dd.onValueChanged.AddListener(OnValChange);
        selection.OptionsUpdated += OnOptionsUpdated;
        selection.LoadingOptions += OnLoadingOptions;
        selection.SelectionChanged += OnSelectionChanged;
        OnOptionsUpdated();
    }

    void OnDisable()
    {
        dd.onValueChanged.RemoveListener(OnValChange);
        selection.OptionsUpdated -= OnOptionsUpdated;
        selection.LoadingOptions -= OnLoadingOptions;
        selection.SelectionChanged -= OnSelectionChanged;
    }

    void OnOptionsUpdated()
    {
        List<string> options = new List<string>();
        for (int i = 0; i < selection.options.Count; i++)
        {
            options.Add(selection.options[i].name);
        }
        dd.ClearOptions();
        dd.AddOptions(options);
        if (selection.selectedIndex >= 0 && selection.selectedIndex < dd.options.Count) dd.value = selection.selectedIndex;
        else dd.value = 0;
    }

    void OnSelectionChanged()
    {
        dd.onValueChanged.RemoveListener(OnValChange);
        dd.value = selection.selectedIndex;
        dd.onValueChanged.AddListener(OnValChange);
    }

    void OnValChange(int v)
    { 
        selection.Select(v);
    }

    private void OnLoadingOptions(bool b)
    {
        dd.interactable = !b;
    }
}

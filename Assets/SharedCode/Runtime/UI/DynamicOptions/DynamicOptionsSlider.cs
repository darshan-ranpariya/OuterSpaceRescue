using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DynamicOptionsSlider : MonoBehaviour
{
    public DynamicOptions selection;
    Slider slider;

    protected void OnEnable()
    {
        if(slider == null) slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValChange);
        selection.OptionsUpdated += OnOptionsUpdated;
        selection.LoadingOptions += OnLoadingOptions;
        selection.SelectionChanged += OnSelectionChanged;
        OnOptionsUpdated();
    }

    protected void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnValChange);
        selection.OptionsUpdated -= OnOptionsUpdated;
        selection.LoadingOptions -= OnLoadingOptions;
        selection.SelectionChanged -= OnSelectionChanged;
    }

    protected virtual void OnOptionsUpdated()
    {
        slider.minValue = 0;
        slider.maxValue = selection.options.Count-1;
        slider.value = 0;
    }

    protected virtual void OnSelectionChanged()
    {
        slider.onValueChanged.RemoveListener(OnValChange);
        slider.value = selection.selectedIndex;
        slider.onValueChanged.AddListener(OnValChange);
    }

    protected virtual void OnValChange(float v)
    { 
        selection.Select(Mathf.FloorToInt(v));
    }

    protected virtual void OnLoadingOptions(bool b)
    {
        slider.interactable = !b;
    }
}

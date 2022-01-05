using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class DynamicOptionsText : MonoBehaviour
{
    public DynamicOptions selection;
    Text text;
    TextMeshProUGUI textPro;
    public bool upperCase;

    void Awake()
    {
        if (text == null) text = GetComponent<Text>();
        if (textPro == null) textPro = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        selection.SelectionChanged += OnSelectionChanged;
        OnSelectionChanged();
    }

    void OnDisable()
    {
        selection.SelectionChanged -= OnSelectionChanged;
    }

    protected virtual void OnSelectionChanged()
    {
        if (upperCase)
        {
            if (text != null) text.text = selection.selectedOption.name.ToUpper();
            if (textPro != null) textPro.text = selection.selectedOption.name.ToUpper();
        }
        else
        {
            if (text != null) text.text = selection.selectedOption.name;
            if (textPro != null) textPro.text = selection.selectedOption.name;
        }
    } 
}

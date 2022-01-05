using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SearchButton : MonoBehaviour 
{ 
    public UIInputField inputField;
    public Button button;
    public UnityEventString evt;

    public bool searchOnInputEdit;

    void OnEnable()
    {
        if (searchOnInputEdit)
        {
            if (inputField != null) inputField.OnValueChanged += Search;
        }
        if (button != null) button.onClick.AddListener(Search);
    }

    void OnDisable()
    {
        if (searchOnInputEdit)
        {
            if (inputField != null) inputField.OnValueChanged -= Search;
        }
        if (button != null) button.onClick.RemoveListener(Search);
    }
    
    private void Search()
    {
        Search(inputField.text);
    }

    private void Search(string arg0)
    {
        evt.Invoke(arg0);
    }
} 
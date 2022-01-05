using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldResetButton : MonoBehaviour, IPointerClickHandler 
{
    public Selectable selectable;
    public UIInputField inputField;
    void OnEnable()
    {
        if (inputField != null)
        {
            inputField.OnValueChanged += InputField_OnValueChanged;
            InputField_OnValueChanged(inputField.text);
        }
    }
    
    void OnDisable()
    {
        if (inputField != null) inputField.OnValueChanged -= InputField_OnValueChanged;
    }

    private void InputField_OnValueChanged(string obj)
    {
        if (selectable != null) selectable.interactable = !string.IsNullOrEmpty(obj);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable != null) selectable.interactable = false;
        if (inputField != null) inputField.text = string.Empty;
    }
} 
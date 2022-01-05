using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldSubmitButton : MonoBehaviour, IPointerClickHandler 
{ 
    public UIInputField inputField;
    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.SubmitEvent.Invoke(inputField.text);
    }
} 
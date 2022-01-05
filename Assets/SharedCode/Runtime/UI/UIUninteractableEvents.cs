using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUninteractableEvents : MonoBehaviour, IPointerClickHandler 
{
    Selectable selectable;
    public UnityEvent onClickEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable == null) selectable = GetComponent<Selectable>();
        if (selectable != null && !selectable.interactable)
        {
            onClickEvent.Invoke();
        }
    }
} 
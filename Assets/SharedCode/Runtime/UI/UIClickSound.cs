using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UIClickSound : MonoBehaviour, IPointerClickHandler
{
    Selectable selectable = null;
    public string soundKey = "BtnClick";
    void Awake()
    {
         selectable = GetComponent<Selectable>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable != null && !selectable.interactable) return;
        AudioPlayer.PlaySFX(soundKey);
    } 
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBtnExt : MonoBehaviour
{
    [HideInInspector]
    public UIButton targetBtn;
    public virtual void Init() { }
    public virtual void OnButtonUpdated() { }
}

public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum State { Normal, Howered, Pressed }
    [SerializeField]
    bool m_interactable = true;
    public bool interactable
    {
        get { return m_interactable; }
        set
        {
            m_interactable = value;
            if(extensions!=null)
                for (int i = 0; i < extensions.Length; i++)
                    extensions[i].OnButtonUpdated();
        }
    }
    [SerializeField]
    State m_state = State.Normal;
    public State state
    {
        get { return m_state; }
        set
        {
            m_state = value;
            if (extensions != null)
                for (int i = 0; i < extensions.Length; i++)
                    extensions[i].OnButtonUpdated();
        }
    }
    UIBtnExt[] extensions;
    public event Action Clicked;
    public event Action<UIButton> ThisClicked;
    public UnityEvent onClick;

    void Awake()
    {
        extensions = GetComponents<UIBtnExt>();
        if (extensions == null) extensions = new UIBtnExt[0];
        for (int i = 0; i < extensions.Length; i++)
        {
            extensions[i].targetBtn = this;
            extensions[i].Init();
        }
        interactable = m_interactable;
        state = m_state;
    } 

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnPointerClick();
    }
    public virtual void OnPointerClick()
    {
        if (interactable)
        {
            if (Clicked != null) Clicked();
            if (ThisClicked != null) ThisClicked(this);
            onClick.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        state = State.Pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        state = State.Howered;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        state = State.Howered;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = State.Normal;
    }
} 
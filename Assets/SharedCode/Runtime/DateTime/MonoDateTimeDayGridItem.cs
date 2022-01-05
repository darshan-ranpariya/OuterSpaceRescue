using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MonoDateTimeDayGridItem : MonoBehaviour, IPointerClickHandler
{
    public Text m_text;
    public Text text
    {
        get
        {
            if(m_text == null) m_text = GetComponent<Text>();
            return m_text;
        }
    }

    public event Action<MonoDateTimeDayGridItem> Clicked;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Clicked!=null)
        {
            Clicked(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class UINumberText : MonoBehaviour, IPointerClickHandler
{ 
    public Text text; 
    public TextMeshProUGUI textPro;

    void Awake()
    {
        if (text == null) text = GetComponent<Text>();
        if (textPro == null) textPro = GetComponent<TextMeshProUGUI>();
        number = m_number;
    }

    public double m_number;
    public double number
    {
        get { return m_number; }
        set
        {
            m_number = value; 
            if (text != null) text.text = value.ToFormattedString(true, maxDigits);
            if (textPro != null) textPro.text = value.ToFormattedString(true, maxDigits);
        }
    }

    public int maxDigits;
    public bool interactive;

    public static void SetText(ref UINumberText[] a, double number)
    {
        if (a!=null)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i].number = number;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactive)
        {
            Toast.Show(number.ToFormattedString(true), transform.position);
        }
    }
}

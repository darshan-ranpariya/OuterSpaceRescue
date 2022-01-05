using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class MonoDateTimeText : MonoDateTimeExtBase
{ 
    [Tooltip("open script for help")]
    public string format = "dd/MM/yyyy";
    DateTimeExt.FormattingHelp help;

    public Text m_text;
    public Text text
    {
        get
        {
            if (m_text == null) m_text = GetComponent<Text>();
            return m_text;
        }
    } 

    public override void OnValChange()
    {
        text.text = refDateTime.selectedTime.ToString(format);
    } 
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class uDateTime : uVar<DateTime, uDateTimeComponent>
{
    public Text[] textComps = new Text[] { };
    public DateTimeText[] dateTimeTextComps = new DateTimeText[] { };
    public UITimeTicker[] tickerComps = new UITimeTicker[] { };
    
    [SerializeField]
    string m_stringValue;
    public string stringValue
    {
        get { return m_stringValue; }
        set
        {
            m_stringValue = value;
            try
            {
                Value = DateTime.Parse(value);
            }
            catch
            {
                Value = DateTime.UtcNow;
            }
        }
    } 

    public override void OnValueChanged()
    {
        m_stringValue = m_value.ToString();

        if (textComps != null)
        {
            for (int i = 0; i < textComps.Length; i++)
            {
                if (textComps[i] != null) textComps[i].text = DateTimeExt.ParseGMTToLocal(m_stringValue).ToString();//(GameStatics.dateTimeFormat);
            }
        } 
        if (dateTimeTextComps != null)
        {
            for (int i = 0; i < dateTimeTextComps.Length; i++)
            {
                if (dateTimeTextComps[i] != null) dateTimeTextComps[i].gmtString = m_stringValue;
            }
        } 
        if (tickerComps != null)
        {
            for (int i = 0; i < tickerComps.Length; i++)
            {
                if (tickerComps[i] != null) tickerComps[i].StartClock(DateTimeExt.ParseGMTToLocal(m_stringValue));
            }
        }
    }
}

public abstract class uDateTimeComponent : MonoBehaviour, IuVarHandler<DateTime>
{
    public abstract void Handle(ref DateTime s);
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(uDateTime))]
public class uDateTimeDrawer : uVarDrawer
{
    string[] arr = new string[] { "textComps", "dateTimeTextComps", "tickerComps", "components" };
    public override string[] arrays
    {
        get
        {
            return arr;
        }
    }

    public override void DrawValueContent(Rect position, SerializedProperty property, GUIContent label)
    { 
        EditorGUI.PropertyField(GetValueArea(GetCurrentRect()), property.FindPropertyRelative("m_stringValue"), GUIContent.none); 
    }
}
#endif
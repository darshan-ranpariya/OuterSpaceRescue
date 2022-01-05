using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DateTimeText : MonoBehaviour
{
    DateTimeExt.FormattingHelp help;
    public string format = string.Empty;
    public bool displayTime = true;
    public bool displayDate = true;

    UnityEngine.UI.Text text;
    TMPro.TextMeshProUGUI textProComp;

    [HideInInspector]
    [SerializeField]
    string _gmtString = "";
    public bool debug;

    public string gmtString
    {
        get { return _gmtString; }
        set
        {
            _gmtString = value;
            UpdateText();
        }
    } 

    void OnEnable()
    {
        if (text == null && textProComp == null)
        {
            text = GetComponent<UnityEngine.UI.Text>();
            textProComp = GetComponent<TMPro.TextMeshProUGUI>();
        }
        if (text == null && textProComp == null) return;

        TimeZoneData.currentTimeZoneOffset.Updated += UpdateText;
        UpdateText();
    }

    void OnDisable()
    {
        TimeZoneData.currentTimeZoneOffset.Updated -= UpdateText;
    }

    void UpdateText()
    {
        if (string.IsNullOrEmpty(_gmtString)) _gmtString = DateTime.UtcNow.ToString();
        //print(TimeZoneData.currentTimeZoneOffset.Value);
        //print(_gmtString); 
        string f = TimeZoneData.dateTimeFormat;
        if (string.IsNullOrEmpty(format))
        {
            if (!displayDate)
            {
                f = TimeZoneData.timeFormat;
            }
            if (!displayTime)
            {
                f = TimeZoneData.dateFormat;
            }
        }
        else f = format;
        if (debug )
        {
            Debug.Log(DateTime.Parse(_gmtString).ToString(f), gameObject);
            Debug.Log(TimeZoneData.currentTimeZoneOffset.Value);
            Debug.Log(TimeZoneData.ParseGMTToLocal(ref _gmtString).ToString(f), gameObject);
        }
        if (text!=null) text.text = TimeZoneData.ParseGMTToLocal(ref _gmtString).ToString(f);
        if (textProComp != null) textProComp.text = TimeZoneData.ParseGMTToLocal(ref _gmtString).ToString(f);
    }

    public static void SetGMTString(ref DateTimeText[] a, string gmtString)
    {
        if (a != null)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i].gmtString = gmtString;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{
    public DateTime dt;
    public TimeSpan offset; 
    public string format = "dddd, d MMM yyyy h:mm";
    DateTimeExt.FormattingHelp help; //see class definition for formating related help

    public Text textComp;
    public TMPro.TextMeshProUGUI textProComp;

    void OnEnable()
    {
        if (textComp == null && textProComp == null)
        {
            textComp = GetComponent<Text>();
            textProComp = GetComponent<TMPro.TextMeshProUGUI>();
        }
        if (textComp == null && textProComp == null) return;

        StopCoroutine("Clock_c");
        StartCoroutine("Clock_c");
        TimeZoneData.currentTimeZoneOffset.Updated += UpdateText;
    }
    void OnDisable()
    {
        TimeZoneData.currentTimeZoneOffset.Updated -= UpdateText;
    }

    private void UpdateText()
    {
        dt = DateTime.UtcNow.Add(TimeZoneData.currentTimeZoneOffset.Value);

        if (textComp != null) textComp.text = dt.ToString(format);
        if (textProComp != null) textProComp.text = dt.ToString(format);
    }

    IEnumerator Clock_c()
    {
        while (true)
        {
            UpdateText();
            yield return new WaitForSeconds(1-(dt.Millisecond/1000f));
        }
    }
}

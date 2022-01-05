using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStringsComponent
{
    event Action StringsUpdated;
    string GetFormattedString(string f);
}

public class ComponentStringText : MonoBehaviour
{
    public Component source;
    public IStringsComponent s;
    public string format;
    public uString text;

    void OnEnable()
    {
        if (s == null) s = (IStringsComponent)source;
        if (s != null)
        {
            s.StringsUpdated += UpdateText;
        }
        UpdateText();
    }

    void OnDisable()
    {
        if (s != null)
        {
            s.StringsUpdated -= UpdateText;
        }
    } 

    void UpdateText()
    {
        if (s != null)
        {
            try
            {
                text.Value = s.GetFormattedString(format);
            }
            catch
            {
                text.Value = string.Empty;
            }
        }
        else
        {
            text.Value = string.Empty;
        }
    }
}

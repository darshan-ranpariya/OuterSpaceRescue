using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uStringComponent_Localized : uStringComponent 
{
    public TMPro.TextMeshProUGUI text;
    string sc = string.Empty;

    void OnEnable()
    {
        Localization.CurrentLanguageUpdated += Localization_CurrentLanguageUpdated;
    }

    void OnDisable()
    {
        Localization.CurrentLanguageUpdated -= Localization_CurrentLanguageUpdated;
    }

    private void Localization_CurrentLanguageUpdated()
    {
        Handle(ref sc);
    }

    public override void Handle(ref string s)
    {
        sc = s;
        if (text == null) text = GetComponent<TMPro.TextMeshProUGUI>();
        if (text != null)
        {
            text.text = Localization.GetString(s);
        }
    }
} 
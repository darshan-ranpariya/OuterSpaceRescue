using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uNumberComponent_CurrencyText : uNumberComponent 
{
    double v = 0;
    TMPro.TextMeshProUGUI text;

    void OnEnable()
    {
        if (text == null) text = GetComponent<TMPro.TextMeshProUGUI>();
        Localization.CurrentLanguageUpdated += UpdateText;
        UpdateText();
    }

    void OnDisable()
    {
        Localization.CurrentLanguageUpdated -= UpdateText;
    }

    private void UpdateText()
    {
        Handle(ref v);
    }

    public override void Handle(ref double s)
    {
        v = s;
        if (text!=null)
        {
            text.text = v.ToCurrencyString();
        }
    }
} 
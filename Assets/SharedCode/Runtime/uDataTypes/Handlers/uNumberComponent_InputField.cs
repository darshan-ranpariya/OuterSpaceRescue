using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uNumberComponent_InputField : uNumberComponent
{
    UIInputField fld;
    
    public override void Handle(ref double s)
    {
        if (fld == null) fld = GetComponent<UIInputField>();
        if (fld != null) fld.text = s.ToFormattedString(true);
    }
} 
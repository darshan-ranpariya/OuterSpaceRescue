using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwExtColor : UISwitchExtension
{
    [HideInInspector]
    public UISwitch uISwitch;
    public MaskableGraphic[] coloredElements;
    public Color onColor, offColor;
    public override void Init(UISwitch uISwt)
    {
        uISwitch = uISwt;
    }

    public override void OnSwitchValChanged(bool isOn)
    {
        for (int i = 0; i < coloredElements.Length; i++)
        {
            if (coloredElements[i]!=null)
            {
                coloredElements[i].color = isOn ? onColor : offColor;
            }
        } 
        
    } 
}

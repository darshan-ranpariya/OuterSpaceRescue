using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwExtHirarchy : UISwitchExtension
{
    public UISwitch uISwitch;
    public Transform targetTrans;

    public override void Init(UISwitch uISw)
    {
        if(uISwitch == null) uISwitch = uISw;
        if (targetTrans == null) targetTrans = this.transform;
    }

    public override void OnSwitchValChanged(bool isOn)
    {
        if (isOn) targetTrans.SetAsLastSibling();
        else
        {
            try
            {
                targetTrans.SetSiblingIndex(targetTrans.parent.childCount - 2);
            }
            catch 
            {
                targetTrans.SetAsFirstSibling();
            }
        }
    } 
}

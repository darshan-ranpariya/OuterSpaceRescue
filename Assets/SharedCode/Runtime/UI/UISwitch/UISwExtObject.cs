using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwExtObject : UISwitchExtension
{
    public GameObject OnObject, OffObject;

    public override void OnSwitchValChanged(bool isOn)
    {
        if (OnObject != null) OnObject.SetActive(isOn);
        if (OffObject != null) OffObject.SetActive(!isOn);
    } 
}

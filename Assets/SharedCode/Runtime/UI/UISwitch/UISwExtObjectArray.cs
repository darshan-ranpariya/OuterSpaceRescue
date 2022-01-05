using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwExtObjectArray : UISwitchExtension
{
    public GameObject[] OnObjects, OffObjects;

    public override void OnSwitchValChanged(bool isOn)
    {
        for (int i = 0; i < OnObjects.Length; i++)
        {
            OnObjects[i].SetActive(isOn);
        }
        for (int i = 0; i < OffObjects.Length; i++)
        {
            OffObjects[i].SetActive(!isOn);
        }
    } 
}

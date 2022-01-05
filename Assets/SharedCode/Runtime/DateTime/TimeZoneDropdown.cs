using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class TimeZoneDropdown : MonoBehaviour
{ 
    Dropdown dd;

    void OnEnable()
    { 
        if (dd==null)
        {
            dd = GetComponent<Dropdown>();
            dd.ClearOptions();
            dd.AddOptions(TimeZoneData.allZones.names);
        }
        dd.onValueChanged.AddListener(DDValChanged);
        dd.value = 0;
    }

    void OnDisable()
    {
        dd.onValueChanged.RemoveListener(DDValChanged);
    }

    void DDValChanged(int v)
    {
        TimeZoneData.SetCurrentTimeZone(v);
    } 
}

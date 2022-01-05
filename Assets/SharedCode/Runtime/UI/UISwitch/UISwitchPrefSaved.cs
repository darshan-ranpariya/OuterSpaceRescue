using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchPrefSaved : UISwitchExtension
{
    public string key;

    public override void Init(UISwitch sw)
    {
        sw._isOn = (PlayerPrefs.GetInt(key) == 1);
    }

    public override void OnSwitchValChanged(bool isOn)
    {
        PlayerPrefs.SetInt(key, (isOn ? 1 : 0));
    }

    //UISwitch _sw;
    //UISwitch sw
    //{
    //    get
    //    {
    //        if (_sw == null) _sw = GetComponent<UISwitch>();
    //        return _sw;
    //    }
    //}
    //bool init = false;

    //void OnEnable()
    //{
    //    sw.Set(PlayerPrefs.GetInt(key) == 1);
    //    sw.Switched += Sw_Switched;
    //    init = true;
    //}

    //void OnDestroy()
    //{
    //    sw.Switched -= Sw_Switched;
    //}

    //private void Sw_Switched(bool obj)
    //{
    //    if(init) PlayerPrefs.SetInt(key, (obj ? 1 : 0));
    //}
}

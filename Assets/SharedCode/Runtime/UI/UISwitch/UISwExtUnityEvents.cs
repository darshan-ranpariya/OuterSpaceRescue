using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISwExtUnityEvents : UISwitchExtension
{
    public UnityEvent onEvent, offEvent;

    public override void OnSwitchValChanged(bool isOn)
    {
        if (isOn) onEvent.Invoke();
        else offEvent.Invoke();
    }
}

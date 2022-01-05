using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class uNumberComponent_UpdateEvent : uNumberComponent
{
    public UnityEvent onValueChanged;
    public override void Handle(ref double s)
    {
        onValueChanged.Invoke();
    }
}

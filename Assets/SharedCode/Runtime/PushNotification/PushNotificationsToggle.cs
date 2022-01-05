using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotificationsToggle : UISliderToggle
{
    public override void Init()
    {
        isOn = PushNotifications.notificationsEnabled;
        base.Init();
    }

    public override void Toggle(bool _isOn)
    {
        Debug.Log("PushNotificationsToggle: " + _isOn);
        if (PushNotifications.SetNotifications(_isOn))
        { 
            base.Toggle(_isOn);
        }
    }
}

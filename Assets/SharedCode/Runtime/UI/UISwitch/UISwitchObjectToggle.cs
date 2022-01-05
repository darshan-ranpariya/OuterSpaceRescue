using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UISwitchObjectToggle : MonoBehaviour
{
    public UISwitch targetSwitch;
    public UISwitch.OnClickAction onEnableAction = UISwitch.OnClickAction.DoNothing;
    public UISwitch.OnClickAction onDisableAction = UISwitch.OnClickAction.DoNothing;

    void OnEnable()
    {
        if (targetSwitch == null) targetSwitch = GetComponent<UISwitch>();
        if (targetSwitch == null) return;

        switch (onEnableAction)
        {
            case UISwitch.OnClickAction.Toggle:
                targetSwitch.Toggle();
                break;
            case UISwitch.OnClickAction.SetOn:
                targetSwitch.Set(true);
                break;
            case UISwitch.OnClickAction.SetOff:
                targetSwitch.Set(false);
                break;
            default:
                break;
        }
    }

    void OnDisable()
    {
        if (targetSwitch == null) return;

        switch (onDisableAction)
        {
            case UISwitch.OnClickAction.Toggle:
                targetSwitch.Toggle();
                break;
            case UISwitch.OnClickAction.SetOn:
                targetSwitch.Set(true);
                break;
            case UISwitch.OnClickAction.SetOff:
                targetSwitch.Set(false);
                break;
            default:
                break;
        }
    }
}

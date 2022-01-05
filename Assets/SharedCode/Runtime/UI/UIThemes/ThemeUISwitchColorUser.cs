using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeUISwitchColorUser : ThemeResourceUserSingleComponent 
    <UISwitchColor, ThemeUISwitchColor, UISwExtColor>
{
    public override void OnAssetUpdated()
    {
        target.onColor = resource.value.OnColor;
        target.offColor= resource.value.OffColor;
        if (target.uISwitch != null) target.OnSwitchValChanged(target.uISwitch.isOn);
    }
}

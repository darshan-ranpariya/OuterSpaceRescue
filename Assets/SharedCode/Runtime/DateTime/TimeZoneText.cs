using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class TimeZoneText : ObservablVariableText<string>
{  
    public override void SetVariable()
    {
        variable = TimeZoneData.currentTimeZone;
    }
}

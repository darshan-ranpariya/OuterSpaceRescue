using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UISwitchColor
{
    public Color OnColor;
    public Color OffColor;
}

[CreateAssetMenu]
public class ThemeUISwitchColor : ThemeResource<UISwitchColor>
{

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwExtParent : UISwitchExtension
{
    public RectTransform childRect;
    public RectTransform onParentRect;
    public RectTransform offParentRectt;
    public bool refit = true;

    public override void OnSwitchValChanged(bool isOn)
    {
        childRect.SetParent(isOn ? onParentRect : offParentRectt);
        if (refit)
        {
            childRect.anchorMin = new Vector2(0, 0);
            childRect.anchorMax = new Vector2(1, 1);
            childRect.offsetMin = new Vector2(0, 0);
            childRect.offsetMax = new Vector2(0, 0);
        }
    }
}

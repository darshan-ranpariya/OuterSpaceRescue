using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICarouselSwitchesExt : UICarouselExtBase
{
    public override void OnSelectionChanged()
    {
        for (int i = 0; i < carousel.items.Length; i++)
        {
            UISwitch s = carousel.items[i].GetComponent<UISwitch>();
            if (s == null) continue;
            s.Set(i == carousel.selectedItemIndex);
        }
    }
}

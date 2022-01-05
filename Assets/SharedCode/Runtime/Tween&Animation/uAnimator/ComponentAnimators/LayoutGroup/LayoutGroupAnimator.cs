using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupAnimator : ComponentAnimator<HorizontalOrVerticalLayoutGroup, LayoutGroupAnimatorState, LayoutGroupAnimatorProperties> {

    public override void AnimateComponent(HorizontalOrVerticalLayoutGroup component, LayoutGroupAnimatorState s1, LayoutGroupAnimatorState s2, float v)
    {
        if(properties.leftPad)
        {
            component.padding.left = Mathf.RoundToInt(Mathf.LerpUnclamped(s1.leftPad, s2.leftPad, v));
        }
        if (properties.rightPad)
        {
            component.padding.right = Mathf.RoundToInt(Mathf.LerpUnclamped(s1.rightPad, s2.rightPad, v));
        }
        if (properties.topPad)
        {
            component.padding.top = Mathf.RoundToInt(Mathf.LerpUnclamped(s1.topPad, s2.topPad, v));
        }
        if (properties.bottomPad)
        {
            component.padding.bottom = Mathf.RoundToInt(Mathf.LerpUnclamped(s1.bottomPad, s2.bottomPad, v));
        }
        if (properties.space)
        {
            component.spacing = Mathf.LerpUnclamped(s1.space, s2.space, v);
        }
    }
}

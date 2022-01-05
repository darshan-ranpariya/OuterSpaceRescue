using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicAnimator : ComponentAnimator<Graphic, GraphicAnimatorState, GraphicAnimatorProperties>
{
    Color c;
    public override void AnimateComponent(Graphic component, GraphicAnimatorState s1, GraphicAnimatorState s2, float v)
    {
        if (properties.color) component.color = Color.LerpUnclamped(s1.color, s2.color, v);
        if (properties.alpha)
        {
            c = component.color;
            c.a = Mathf.LerpUnclamped(s1.alpha, s2.alpha, v);
            component.color = c;
        }
    }
} 
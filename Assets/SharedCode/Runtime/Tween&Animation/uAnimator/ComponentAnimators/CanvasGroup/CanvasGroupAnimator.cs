using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupAnimator : ComponentAnimator<CanvasGroup, CanvasGroupAnimatorState, CanvasGroupAnimatorProperties>
{ 
    public override void AnimateComponent(CanvasGroup component, CanvasGroupAnimatorState s1, CanvasGroupAnimatorState s2, float v)
    {
        component.alpha = Mathf.LerpUnclamped(s1.alpha, s2.alpha, v);
    }
} 
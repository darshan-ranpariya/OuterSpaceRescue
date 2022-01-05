using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMeshAnimator : ComponentAnimator<Text, TextMeshAnimatorState, TextMeshAnimatorProperties>
{
    public override void AnimateComponent(Text component, TextMeshAnimatorState s1, TextMeshAnimatorState s2, float v)
    {
        component.fontSize = (int)Mathf.LerpUnclamped(s1.fontSize, s2.fontSize, v);
    }
} 
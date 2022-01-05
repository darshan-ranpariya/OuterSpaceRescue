using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformAnimator1 : ComponentAnimator<Transform, TransformAnimatorState1, TransformAnimatorProperties1>
{
    public override void AnimateComponent(Transform component, TransformAnimatorState1 s1, TransformAnimatorState1 s2, float v)
    {
        switch (properties.position)
        {
            case TransformAnimatorProperties1.Space.Local:
                component.localPosition = Vector3.LerpUnclamped(s1.position, s2.position, v);
                break;
            case TransformAnimatorProperties1.Space.Global:
                component.position = Vector3.LerpUnclamped(s1.position, s2.position, v);
                break;
            default:
                break;
        }

        switch (properties.rotation)
        {
            case TransformAnimatorProperties1.Space.Local:
                component.localEulerAngles = Vector3.LerpUnclamped(s1.eulerAngles, s2.eulerAngles, v);
                break;
            case TransformAnimatorProperties1.Space.Global:
                component.eulerAngles = Vector3.LerpUnclamped(s1.eulerAngles, s2.eulerAngles, v);
                break;
            default:
                break;
        }

        switch (properties.scale)
        {
            case TransformAnimatorProperties1.Space.Local:
                component.localScale = Vector3.LerpUnclamped(s1.scale, s2.scale, v);
                break;
            default:
                break;
        }
    }  
} 
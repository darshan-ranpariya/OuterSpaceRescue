using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformAnimator : ComponentAnimator<Transform, TransformAnimatorState, TransformAnimatorProperties>
{
    public override void AnimateComponent(Transform component, TransformAnimatorState s1, TransformAnimatorState s2, float v)
    {
        switch (properties.position)
        {
            case TransformAnimatorProperties.Space.Local:
                component.localPosition = Vector3.LerpUnclamped(s1.source.localPosition, s2.source.localPosition, v);
                break;
            case TransformAnimatorProperties.Space.Global:
                component.position = Vector3.LerpUnclamped(s1.source.position, s2.source.position, v);
                break;
            default:
                break;
        }

        switch (properties.rotation)
        {
            case TransformAnimatorProperties.Space.Local:
                component.localRotation = Quaternion.LerpUnclamped(s1.source.localRotation, s2.source.localRotation, v);
                break;
            case TransformAnimatorProperties.Space.Global:
                component.rotation = Quaternion.LerpUnclamped(s1.source.rotation, s2.source.rotation, v);
                break;
            default:
                break;
        }

        switch (properties.scale)
        {
            case TransformAnimatorProperties.Space.Local:
                component.localScale = Vector3.LerpUnclamped(s1.source.localScale, s2.source.localScale, v);
                break;
            default:
                break;
        }
    }  
} 
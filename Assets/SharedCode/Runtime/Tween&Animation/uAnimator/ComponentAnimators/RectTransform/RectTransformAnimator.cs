using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformAnimator : ComponentAnimator<RectTransform, RectTransformAnimatorState, RectTransformAnimatorProperties>
{
    [System.Serializable]
    public class RectGlobals
    {
        public float top = 0;
        public float bottom = 0;
        public float left = 0;
        public float right = 0;
    } 

    RectGlobals targetStartRectGlobals = new RectGlobals();
    RectGlobals targetEndRectGlobals = new RectGlobals();
    RectGlobals blendedRectGlobals = new RectGlobals();

    public override void AnimateComponent(RectTransform component, RectTransformAnimatorState s1, RectTransformAnimatorState s2, float v)
    { 
        UpdateRectGlobals(ref targetStartRectGlobals, s1.source);
        UpdateRectGlobals(ref targetEndRectGlobals, s2.source);
        blendedRectGlobals.top = Mathf.LerpUnclamped(targetStartRectGlobals.top, targetEndRectGlobals.top, v);
        blendedRectGlobals.bottom = Mathf.LerpUnclamped(targetStartRectGlobals.bottom, targetEndRectGlobals.bottom, v);
        blendedRectGlobals.right = Mathf.LerpUnclamped(targetStartRectGlobals.right, targetEndRectGlobals.right, v);
        blendedRectGlobals.left = Mathf.LerpUnclamped(targetStartRectGlobals.left, targetEndRectGlobals.left, v);

        if (properties.position)
        {
            component.anchorMin = Vector2.LerpUnclamped(s1.source.anchorMin, s2.source.anchorMin, v);
            component.anchorMax = Vector2.LerpUnclamped(s1.source.anchorMax, s2.source.anchorMax, v);
            component.pivot = Vector2.LerpUnclamped(s1.source.pivot, s2.source.pivot, v);
            component.position = Vector3.LerpUnclamped(s1.source.position, s2.source.position, v);
        }

        if (properties.size)
        {
            component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (blendedRectGlobals.right - blendedRectGlobals.left)/component.lossyScale.x);
            component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (blendedRectGlobals.top - blendedRectGlobals.bottom) / component.lossyScale.y);
        }

        if (properties.rotation)
        {
            component.rotation = Quaternion.LerpUnclamped(s1.source.rotation, s2.source.rotation, v);
        }
    }  

    void UpdateRectGlobals(ref RectGlobals g, RectTransform r)
    {
        g.top = r.position.y + (r.rect.yMax * r.lossyScale.y);
        g.bottom = r.position.y + (r.rect.yMin * r.lossyScale.y);
        g.right = r.position.x + (r.rect.xMax * r.lossyScale.x);
        g.left = r.position.x + (r.rect.xMin * r.lossyScale.x);
    } 
} 
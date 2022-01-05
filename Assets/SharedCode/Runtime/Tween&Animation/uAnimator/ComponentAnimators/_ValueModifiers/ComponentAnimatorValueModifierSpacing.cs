using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentAnimatorValueModifierSpacing : ComponentAnimatorValueModifier
{
    [Range(0,.5f)]
    public float spacing = 0.1f;
    float s = 0;
    float p = 0;

    public override float GetModifiedValue(int componentIndex, int totalComponents, float value)
    { 
        s = spacing;
        p = value;
        if (totalComponents * s > .5f) s = .5f / totalComponents;

        float startP = s * componentIndex;
        float endP = 1f - (s * (totalComponents - componentIndex));
        float range = endP - startP; 
        p = Mathf.Lerp(0, 1, (value - startP)/range);
        //Debug.LogFormat("{0}/{1} {2}->{3}", componentIndex, totalComponents, value, p);
        return p;
    } 
} 
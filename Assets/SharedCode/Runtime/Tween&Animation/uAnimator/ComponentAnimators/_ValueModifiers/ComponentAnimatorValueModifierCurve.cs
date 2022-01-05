using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentAnimatorValueModifierCurve : ComponentAnimatorValueModifier
{
    public AnimationCurveAsset curve;

    public override float GetModifiedValue(int componentIndex, int totalComponents, float value)
    {
        return (curve != null) ? curve.Evaluate(value) : value;
    }
} 
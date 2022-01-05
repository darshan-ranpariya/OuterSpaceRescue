using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uNumberAnimator : uAnimator
{
    public uNumber number;
    public double startValue;
    public double endValue;
    public override void Animate(float value)
    {
        if (value == 0) number.Value = startValue;
        else if (value == 1) number.Value = endValue;
        else number.Value = startValue + System.Math.Floor((endValue - startValue) * value);
    }
} 
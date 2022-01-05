using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectStateAnimator : uAnimator
{
    [MinMax(0,1)]
    public Vector2 range;

    public override void Animate(float value)
    {
        float v = (value - range.x) / (range.y - range.x);
        gameObject.SetActive(v >= 0 && v <= 1);
    }
} 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformParentAnimator : uAnimator
{
    [Serializable]
    public class TransformParent
    {
        public Transform parent;
        [MinMax(0,1)]
        public Vector2 range;
    }

    public TransformParent[] parents;

    public override void Animate(float value)
    {
        foreach (var obj in parents)
        {
            float v = (value - obj.range.x) / (obj.range.y - obj.range.x);
            if(v >= 0 && v <= 1) transform.SetParent(obj.parent);
        }
    }
} 
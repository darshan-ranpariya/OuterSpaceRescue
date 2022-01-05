using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uAnimatorBlender : uAnimator
{
    [System.Serializable]
    public class Blend
    {
        public string name;
        public uAnimator animator;
        [MinMax(0, 1)]
        public Vector2 range;
    }

    public Blend[] animators;
    public override void Animate(float value)
    {
        float v = 0;
        for (int i = 0; i < animators.Length; i++)
        {
            v = value;
            if (animators[i].animator != null)
            {
                v = (v - animators[i].range.x) / (animators[i].range.y - animators[i].range.x);
                animators[i].animator.value = Mathf.Clamp(v, 0, 1);
            }
        }
    }
} 
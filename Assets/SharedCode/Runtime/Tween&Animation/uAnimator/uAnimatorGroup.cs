using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uAnimatorGroup : uAnimator
{
    public uAnimator[] animators = new uAnimator[0];
    public override void Animate(float value)
    {
        for (int i = 0; i < animators.Length; i++)
        {
           if(animators[i]!=null) animators[i].value = value;
        }
    }
} 


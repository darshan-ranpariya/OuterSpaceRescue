using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwExtAnimator : UISwitchExtension
{
    public uAnimatorPlayer onAnimator;
    public uAnimatorPlayer offAnimator;

    public override void OnSwitchValChanged(bool isOn)
    {
        if (!Application.isPlaying) return;
        if(onAnimator != null) onAnimator.Stop();
        if (offAnimator != null) offAnimator.Stop();
        if (isOn)
        {
            if (onAnimator != null) onAnimator.Restart();
        }
        else
        {
            if (offAnimator != null) offAnimator.Restart();
        }
    } 
}

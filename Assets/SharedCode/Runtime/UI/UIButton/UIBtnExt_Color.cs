using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnExt_Color : UIBtnExt
{
    public Color normalColor;
    public Color howeredColor;
    public Color pressedColor;
    public Color disabledColor;

    public uColor color;

    public override void OnButtonUpdated()
    {
        if (targetBtn.interactable)
        {
            switch (targetBtn.state)
            {
                case UIButton.State.Normal:
                    color.Value = normalColor;
                    break;
                case UIButton.State.Howered:
                    color.Value = howeredColor;
                    break;
                case UIButton.State.Pressed:
                    color.Value = pressedColor;
                    break;
                default:
                    break;
            }
        }
        else
        {
            color.Value = disabledColor;
        }
    }
} 
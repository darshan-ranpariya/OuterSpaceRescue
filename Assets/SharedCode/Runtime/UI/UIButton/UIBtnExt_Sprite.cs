using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnExt_Sprite : UIBtnExt
{
    public Sprite normalSprite;
    public Sprite howeredSprite;
    public Sprite pressedSprite;
    public Sprite disabledSprite;

    public uImage image;

    public override void OnButtonUpdated()
    {
        if (targetBtn.interactable)
        {
            switch (targetBtn.state)
            {
                case UIButton.State.Normal:
                    image.Value = normalSprite;
                    break;
                case UIButton.State.Howered:
                    image.Value = howeredSprite;
                    break;
                case UIButton.State.Pressed:
                    image.Value = pressedSprite;
                    break;
                default:
                    break;
            }
        }
        else
        {
            image.Value = disabledSprite;
        }
    }
} 
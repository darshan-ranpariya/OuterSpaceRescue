using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnExt_Objects : UIBtnExt
{
    public enum ButtonState { Normal, Howered, Pressed, Disabled}
    [Serializable]
    public class uEnumButtonStateCond : uEnumCond<ButtonState> { }
    [Serializable]
    public class uEnumButtonState : uEnum<ButtonState, uEnumButtonStateCond> { }

    public uEnumButtonState objectsSwitch;
    public override void OnButtonUpdated()
    {
        if (targetBtn.interactable)
        {
            switch (targetBtn.state)
            {
                case UIButton.State.Normal:
                    objectsSwitch.value = ButtonState.Normal;
                    break;
                case UIButton.State.Howered:
                    objectsSwitch.value = ButtonState.Howered;

                    break;
                case UIButton.State.Pressed:
                    objectsSwitch.value = ButtonState.Pressed;
                    break;
                default:
                    break;
            }
        }
        else
        {
            objectsSwitch.value = ButtonState.Disabled;
        }
    }
} 
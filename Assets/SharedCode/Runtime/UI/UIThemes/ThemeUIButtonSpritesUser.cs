using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeUIButtonSpritesUser : ThemeResourceUserSingleComponent<UIButtonSprites, ThemeUIButtonSprites, UIBtnExt_Sprite>
{
    public override void OnAssetUpdated()
    {
        target.normalSprite = resource.value.normalSprite;
        target.howeredSprite = resource.value.howeredSprite;
        target.pressedSprite = resource.value.pressedSprite;
        target.disabledSprite = resource.value.disabledSprite;
        target.OnButtonUpdated();
    }
} 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIButtonSprites
{
    public Sprite normalSprite;
    public Sprite howeredSprite;
    public Sprite pressedSprite;
    public Sprite disabledSprite;
}

[CreateAssetMenu]
public class ThemeUIButtonSprites : ThemeResource<UIButtonSprites>
{

} 
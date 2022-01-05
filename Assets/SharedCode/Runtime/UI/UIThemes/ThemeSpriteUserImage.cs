using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSpriteUserImage : ThemeSpriteUserSingleComponent<Image>
{
    public override void OnAssetUpdated()
    {
        target.sprite = resource.value;
    }
} 
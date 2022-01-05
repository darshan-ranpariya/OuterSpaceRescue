using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu()]
public class ThemeSprite : ThemeResource<Sprite>
{

}
public abstract class ThemeSpriteUserSingleComponent<TTarget> : ThemeResourceUserSingleComponent<Sprite, ThemeSprite, TTarget> where TTarget : Component
{
    public abstract override void OnAssetUpdated();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class ThemeColor : ThemeResource<Color>
{

}

public abstract class ThemeColorUserSingleComponent<TComp> : ThemeResourceUserSingleComponent<Color, ThemeColor, TComp> where TComp : Component
{
    public abstract override void OnAssetUpdated();
}

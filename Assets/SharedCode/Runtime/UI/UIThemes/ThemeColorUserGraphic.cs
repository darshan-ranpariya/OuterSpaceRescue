using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeColorUserGraphic : ThemeColorUserSingleComponent<Graphic>
{
    [Range(0,1)]
    public float alpha = 1;
    public override void OnAssetUpdated()
    {
        target.color = resource.value.transparent(1-alpha);
    }
}

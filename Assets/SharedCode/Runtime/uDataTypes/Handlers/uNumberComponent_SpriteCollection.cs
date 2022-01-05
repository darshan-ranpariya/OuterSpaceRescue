using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uNumberComponent_SpriteCollection : uNumberComponent
{
    public SpriteCollection spriteCollection;
    public Image img;

    public bool numberAsKey;

    public override void Handle(ref double n)
    {
        try
        {
            if(numberAsKey) img.sprite = spriteCollection.GetSprite(n.ToString()); 
            else img.sprite = spriteCollection.GetSprite((int)n);
        }
        catch { }
    }
    void OnValidate()
    {
        if (img == null) img = GetComponent<Image>();
    }
}

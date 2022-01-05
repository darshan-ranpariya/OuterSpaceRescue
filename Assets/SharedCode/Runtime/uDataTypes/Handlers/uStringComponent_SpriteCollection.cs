using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uStringComponent_SpriteCollection : uStringComponent 
{
    public SpriteCollection collection;
    public Image img;

    public override void Handle(ref string s)
    {
        if (img!=null)
        {
            Sprite sp = null;
            if (collection != null) sp = collection.GetSprite(s);
            if (sp != null) img.sprite = sp;
            else img.sprite = SpriteCollection.transparentSprite;
        }
    } 

    void OnValidate()
    {
        if (img == null) img = GetComponent<Image>();
    }
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uBoolComponent_GraphicColor : uBoolComponent
{
    public Graphic graphic;
    public Color onTrueColor;
    public Color onFalseColor;

    public override void Handle(ref bool s)
    {
        if (graphic!=null)
        {
            graphic.color = s ? onTrueColor : onFalseColor;
        }
    }

    void OnValidate()
    {
        if (graphic == null) graphic = GetComponent<Graphic>();
    }
}

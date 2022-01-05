using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uStringComponent_ColorCollection : uStringComponent 
{
    public ColorCollection collection;
    public Graphic graphic;

    public override void Handle(ref string s)
    {
        if (graphic!=null)
        { 
            if(collection!=null) graphic.color = collection.GetColor(s);
        }
    } 

    void OnValidate()
    {
        if (graphic == null) graphic = GetComponent<Graphic>();
    }
} 
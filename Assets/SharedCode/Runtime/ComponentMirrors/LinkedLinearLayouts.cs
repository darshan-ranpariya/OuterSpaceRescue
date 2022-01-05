using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkedLinearLayouts : LinkedComponentsBase<LinearLayoutGroup>
{ 
    [Header("Linked Properties")]
    public bool maxWidth = true;
    public bool childAlignment = false;

    public override void LinkComp(LinearLayoutGroup refComp, LinearLayoutGroup targetComp)
    {
        targetComp.maxWidth = refComp.maxWidth;
        targetComp.childAlignment = refComp.childAlignment; 
    }
}
   

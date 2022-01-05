using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonoDateTimeAddBtn : MonoDateTimeExtBase, IPointerClickHandler
{
    public int addDays;
    public int addMonths;
    public int addYears;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (addYears != 0)
        {
            refDateTime.AddYears(addYears);
        }
        if (addMonths != 0)
        {
            refDateTime.AddMonths(addMonths);
        }
        if (addDays!=0)
        {
            refDateTime.AddDays(addDays);
        }
    }

    public override void OnValChange()
    {
        
    }
}

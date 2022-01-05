using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoDateTimeExtGroup : MonoBehaviour
{
    public MonoDateTime refDateTime;
    public MonoDateTimeExtBase[] exts; 

    public void ActivateGroup(MonoDateTime refDateTime)
    {
        this.refDateTime = refDateTime;
        for (int i = 0; i < exts.Length; i++)
        {
            exts[i].SetRefDateTime(refDateTime);
        }
        gameObject.SetActive(true);
    }

    public void DeactivateGroup()
    {
        gameObject.SetActive(false);
    }

    public void ToggleGroup(MonoDateTime refDateTime)
    {
        if (gameObject.activeSelf)
        {
            DeactivateGroup();
        }
        else
        {
            ActivateGroup(refDateTime);
        }
    }
}

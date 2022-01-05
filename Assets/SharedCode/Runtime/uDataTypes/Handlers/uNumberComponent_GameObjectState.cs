using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uNumberComponent_GameObjectState : uNumberComponent 
{
    public enum Rule { LessThan, LessThanOrEqual, Equal, GreaterThanOrEqual, GreaterThan }
    public Rule ActiveRule = Rule.Equal;
    public double refNumber = 0;

    public override void Handle(ref double s)
    {
        switch (ActiveRule)
        {
            case Rule.LessThan:
                gameObject.SetActive(s < refNumber);
                break;
            case Rule.LessThanOrEqual:
                gameObject.SetActive(s <= refNumber);
                break;
            case Rule.Equal:
                gameObject.SetActive(s == refNumber);
                break;
            case Rule.GreaterThanOrEqual:
                gameObject.SetActive(s >= refNumber);
                break;
            case Rule.GreaterThan:
                gameObject.SetActive(s > refNumber);
                break;
            default:
                break;
        }
    }
} 
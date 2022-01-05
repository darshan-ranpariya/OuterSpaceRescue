using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uStringComponent_ValueHolder : uStringComponent {

    [HideInInspector]
    public string Value;

    public override void Handle(ref string s)
    {
        Value = s;
    }
}

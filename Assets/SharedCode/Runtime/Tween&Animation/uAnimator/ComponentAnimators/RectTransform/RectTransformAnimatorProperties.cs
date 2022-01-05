using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RectTransformAnimatorState : ComponentAnimatorState
{
    public RectTransform source;
}

public class RectTransformAnimatorProperties : ComponentAnimatorProperties<RectTransformAnimatorState>
{ 
    public bool position;
    public bool rotation;
    public bool size;
} 
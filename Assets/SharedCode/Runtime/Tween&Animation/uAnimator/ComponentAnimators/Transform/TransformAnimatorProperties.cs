using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TransformAnimatorState : ComponentAnimatorState
{
    public Transform source;
}

public class TransformAnimatorProperties : ComponentAnimatorProperties<TransformAnimatorState>
{
    public enum Space { None, Local, Global }

    public Space position;
    public Space rotation;
    public Space scale;
} 
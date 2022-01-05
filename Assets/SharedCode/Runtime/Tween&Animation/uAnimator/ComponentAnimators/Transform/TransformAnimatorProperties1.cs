using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TransformAnimatorState1 : ComponentAnimatorState
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;
}

public class TransformAnimatorProperties1 : ComponentAnimatorProperties<TransformAnimatorState1>
{
    public enum Space { None, Local, Global }

    public Space position;
    public Space rotation;
    public Space scale;
} 
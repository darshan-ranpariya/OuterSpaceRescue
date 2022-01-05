using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TextMeshAnimatorState : ComponentAnimatorState
{
    public float fontSize;
}

public class TextMeshAnimatorProperties : ComponentAnimatorProperties<TextMeshAnimatorState>
{

} 
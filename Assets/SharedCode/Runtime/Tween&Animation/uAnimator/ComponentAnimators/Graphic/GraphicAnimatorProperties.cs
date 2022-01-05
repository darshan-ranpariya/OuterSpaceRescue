using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GraphicAnimatorState : ComponentAnimatorState
{
    public Color color;
    public float alpha;
}

public class GraphicAnimatorProperties : ComponentAnimatorProperties<GraphicAnimatorState>
{
    public bool color;
    public bool alpha;
} 
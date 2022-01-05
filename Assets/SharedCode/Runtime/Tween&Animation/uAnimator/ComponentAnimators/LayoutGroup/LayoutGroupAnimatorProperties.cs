using System;
using UnityEngine.UI;

[Serializable]
public class LayoutGroupAnimatorState : ComponentAnimatorState
{
    public float leftPad;
    public float rightPad;
    public float topPad;
    public float bottomPad;
    public float space;
}

public class LayoutGroupAnimatorProperties : ComponentAnimatorProperties<LayoutGroupAnimatorState>
{
    public bool leftPad;
    public bool rightPad;
    public bool topPad;
    public bool bottomPad;
    public bool space;
}

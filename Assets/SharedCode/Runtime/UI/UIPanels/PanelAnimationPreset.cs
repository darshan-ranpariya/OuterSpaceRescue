using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PanelAnimationPreset", menuName = "Animations/PanelAnimationPreset")]
public class PanelAnimationPreset : ScriptableObject
{
    public AnimationCurveAsset openCurve;
    public AnimationCurveAsset closeCurve;
    [Space]
    public float openDuration;
    public float closeDuration;
    [Space]
    public Vector3 openPos;
    public Vector3 closePos;
    [Space] 
    public Vector3 openRot;
    public Vector3 closeRot;
    [Space]
    public Vector3 openScale;
    public Vector3 closeScale;

    public bool fade;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : AnimationBase
{
    public Panel panel;
    public PanelAnimationPreset preset;
    public CanvasGroup canvasGroup;
    public Transform targetTransform;

    public override float Duration
    {
        get
        {
            if (preset != null && panel!=null)
            {
                return panel.isActive ? preset.closeDuration : preset.openDuration;
            }
            return base.Duration;
        }

        set
        {
            base.Duration = value;
        }
    }

    public override void StartAnim()
    {
        StopCoroutine("Anim_c");
        StartCoroutine("Anim_c");
    }

    public override void StopAnim()
    {
        StopCoroutine("Anim_c");
    }

    public override void ResetAnim()
    {
        StopCoroutine("Anim_c");
        et = 0;
        ApplyVal();
    }

    float et;
    float tt;
    bool o;
    bool p;
    bool r;
    bool s;
    bool a;
    Vector3 sp;
    Vector3 ep;
    Vector3 sr;
    Vector3 er;
    Vector3 ss;
    Vector3 es;
    float sa;
    float ea;
    AnimationCurve curve;

    IEnumerator Anim_c()
    {
        if (panel == null) yield break;
        if (targetTransform == null) yield break;


        o = panel.isActive;

        et = 0;
        tt = o ? preset.openDuration : preset.closeDuration;
        sp = o ? preset.closePos : preset.openPos;
        ep = o ? preset.openPos : preset.closePos;
        sr = o ? preset.closeRot : preset.openRot;
        er = o ? preset.openRot : preset.closeRot;
        ss = o ? preset.closeScale : preset.openScale;
        es = o ? preset.openScale : preset.closeScale;
        sa = o ? 0 : 1;
        ea = o ? 1 : 0;
        curve = o ? preset.openCurve.curve : preset.closeCurve.curve;

        p = !sp.Equals(ep);
        r = !sr.Equals(er);
        s = !ss.Equals(es);
        a = canvasGroup != null;
         
        while (et < tt)
        {
            ApplyVal();
            et += Time.deltaTime; 
            yield return null;
        }

        et = tt;
        ApplyVal();
        onEnd.Invoke();
    }

    void ApplyVal()
    {
        float cev = 0;
        if(curve!=null && tt>0) cev = curve.Evaluate(et / tt);
        if (p)
        {
            if(targetTransform is RectTransform) ((RectTransform)targetTransform).anchoredPosition = Vector3.LerpUnclamped(sp, ep, cev);
            else targetTransform.localPosition = Vector3.LerpUnclamped(sp, ep, cev);
        }
        if (r) targetTransform.localEulerAngles = Vector3.LerpUnclamped(sr, er, cev);
        if (s) targetTransform.localScale = Vector3.LerpUnclamped(ss, es, cev);
        if (a) canvasGroup.alpha = Mathf.Lerp(sa, ea, cev);
    }

    void OnValidate()
    {
        if (targetTransform == null) targetTransform = transform;

        if (panel == null)
        {
            panel = GetComponent<Panel>();
        }

        if (panel != null)
        {
            panel.openAnim = this;
            panel.closeAnim = this;

            if (preset!=null)
            {
                if (preset.fade)
                {
                    if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
                    if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }
    }
}

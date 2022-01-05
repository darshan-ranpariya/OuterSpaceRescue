using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uDateTimeComponent_ImageFiller : uDateTimeComponent
{
    public Image imgFill;
    DateTime leftSideTime;
    DateTime rightSideTime;
    public bool utc = true;

    void OnEnable()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (imgFill == null) imgFill = GetComponent<Image>();
        if (imgFill == null) return;
    }

    public void UpdateValues(DateTime _startTime, DateTime _endTime)
    {
        UpdateValues(_startTime, _endTime, utc);
    }

    public void UpdateValues(DateTime _startTime, DateTime _endTime, bool _utc)
    {
        leftSideTime = _startTime;
        rightSideTime = _endTime;
        utc = _utc;
    }

    float v = 0;
    void Update()
    {
        if (imgFill == null) return;

        float t = (float)(rightSideTime.Subtract(leftSideTime).TotalSeconds);
        float d = 0;
        if (utc) d = (float)(DateTime.UtcNow.Subtract(leftSideTime).TotalSeconds);
        else d = (float)(DateTime.Now.Subtract(leftSideTime).TotalSeconds);
        v = Mathf.Clamp(d / t, 0, 1);
        if (imgFill.fillAmount != v) imgFill.fillAmount = v;
    }

    public string testLeftTime;
    public string testRightTime;
    public void Test()
    {
        DateTime dtl = DateTime.Parse(testLeftTime);
        DateTime dtr = DateTime.Parse(testRightTime);
        UpdateValues(dtl, dtr);
    }

    public override void Handle(ref DateTime s)
    {
        if (imgFill == null) return;

        float t = (float)(s.Subtract(leftSideTime).TotalSeconds);
        float d = 0;
        if (utc) d = (float)(DateTime.UtcNow.Subtract(leftSideTime).TotalSeconds);
        else d = (float)(DateTime.Now.Subtract(leftSideTime).TotalSeconds);
        v = Mathf.Clamp(d / t, 0, 1);
        if (imgFill.fillAmount != v) imgFill.fillAmount = v;
    }
}

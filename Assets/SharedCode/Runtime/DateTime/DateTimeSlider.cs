using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateTimeSlider : MonoBehaviour
{
    public Slider slider;
    DateTime leftSideTime;
    DateTime rightSideTime;
    public bool utc = true;

    void OnEnable()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (slider == null) slider = GetComponent<Slider>();
        if (slider == null) return;
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.direction = Slider.Direction.LeftToRight;
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
        if (slider == null) return;

        float t = (float)(rightSideTime.Subtract(leftSideTime).TotalSeconds);
        float d = 0;
        if(utc) d = (float)(DateTime.UtcNow.Subtract(leftSideTime).TotalSeconds);
        else d = (float)(DateTime.Now.Subtract(leftSideTime).TotalSeconds);
        v = Mathf.Clamp(d / t, 0, 1);  
        if (slider.value != v) slider.value = v;
    }

    public string testLeftTime;
    public string testRightTime;
    public void Test()
    {
        DateTime dtl = DateTime.Parse(testLeftTime);
        DateTime dtr = DateTime.Parse(testRightTime);
        UpdateValues(dtl, dtr);
    }
} 
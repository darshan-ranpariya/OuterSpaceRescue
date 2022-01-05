using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoDateTime : MonoBehaviour
{ 
    public string minVal = "2018";
    public string maxVal = "";
    public string defaultVal = "";

    DateTime minValDT;
    DateTime maxValDT;
    public DateTime selectedTime;
    public event Action ValChanged;

    public string selectedTimeString = "";

    public void OnEnable()
    {
        try
        {
            minValDT = DateTime.Parse(minVal);
        }
        catch
        {
            //minValDT = DateTime.Now;
        }
        minVal = minValDT.ToString();

        try
        {
            maxValDT = DateTime.Parse(maxVal);
        }
        catch
        {
            maxValDT = DateTime.Now;
        }
        maxVal = maxValDT.ToString();

        if (selectedTime.Year > 1)
        {
            SetDate(selectedTime);
        }
        else
        {
            try
            {
                SetDate(DateTime.Parse(defaultVal));
            }
            catch
            {
                SetDate(DateTime.Now);
            }
        }
    }

    public void SetDate(DateTime dateTime)
    {
        if (dateTime.Subtract(minValDT).TotalSeconds < 0) dateTime = minValDT;
        if (maxValDT.Subtract(dateTime).TotalSeconds < 0) dateTime = maxValDT;
        selectedTime = dateTime;
        selectedTimeString = selectedTime.ToString();
        if (ValChanged != null) ValChanged();
    }

    public void SetYear(int y)
    {
        int d = selectedTime.Day;
        selectedTime = new DateTime(y, selectedTime.Month, 1);
        SetDayOfMonth(d);
    }

    public void SetMonth(int m)
    {
        print("2");
        m = Mathf.Clamp(m, 1, 12);

        int d = selectedTime.Day;
        selectedTime = new DateTime(selectedTime.Year, m, 1);
        SetDayOfMonth(d);
    }

    public void SetDayOfMonth(int d)
    {
        print("3");
        d = Mathf.Clamp(d, 1, DateTime.DaysInMonth(selectedTime.Year, selectedTime.Month));

        SetDate(new DateTime(selectedTime.Year, selectedTime.Month, d)); 
    }

    public void AddYears(int y)
    {
        SetDate(selectedTime.AddYears(y));
    }

    public void AddMonths(int m)
    {
        SetDate(selectedTime.AddMonths(m));
    }

    public void AddDays(double d)
    {
        SetDate(selectedTime.AddDays(d));
    }


}
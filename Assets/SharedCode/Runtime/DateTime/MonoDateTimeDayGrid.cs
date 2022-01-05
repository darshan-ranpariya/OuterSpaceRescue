using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoDateTimeDayGrid : MonoDateTimeExtBase
{ 
    public DayOfWeek startDay;
    public Color inactiveDaysColor = Color.white, activeDaysColor = Color.white, selectedDayColor = Color.white;
    DateTime gridStartTime;

    public List<MonoDateTimeDayGridItem> gridItems;


    public override void OnEnableBase()
    {
        for (int i = 0; i < gridItems.Count; i++)
        {
            gridItems[i].Clicked += OnGridItemClicked;
        } 
        OnValChange();
    }

    public override void OnDisableBase()
    {
        for (int i = 0; i < gridItems.Count; i++)
        {
            gridItems[i].Clicked -= OnGridItemClicked;
        }
    }

    public override void OnValChange()
    {
        gridStartTime = new DateTime(refDateTime.selectedTime.Year, refDateTime.selectedTime.Month, 1);
        int firstDayOfMonth = (int)gridStartTime.DayOfWeek;
        int firstDayOfGrid = (int)startDay;
        if (firstDayOfGrid > firstDayOfMonth) firstDayOfMonth += 7;
        gridStartTime = gridStartTime.Subtract(TimeSpan.FromDays(firstDayOfMonth - firstDayOfGrid));

        DateTime tempDT;
        for (int i = 0; i < gridItems.Count; i++)
        {
            tempDT = gridStartTime.AddDays(i);
            gridItems[i].text.text = tempDT.Day.ToString();
            if (gridItems[i].text.text.Length == 1) gridItems[i].text.text = string.Format(" {0}", gridItems[i].text.text);
        }
        UpdateColors();
    }

    void UpdateColors()
    {
        DateTime tempDT;
        for (int i = 0; i < gridItems.Count; i++)
        {
            tempDT = gridStartTime.AddDays(i);
            if (tempDT.Year != refDateTime.selectedTime.Year || tempDT.Month != refDateTime.selectedTime.Month)
            {
                gridItems[i].text.color = inactiveDaysColor;
                continue;
            }

            if (tempDT.Day != refDateTime.selectedTime.Day)
            {
                gridItems[i].text.color = activeDaysColor;
                continue;
            }

            gridItems[i].text.color = selectedDayColor;
        }
    }

    void OnGridItemClicked(MonoDateTimeDayGridItem item)
    {
        DateTime tempDT = gridStartTime.AddDays(gridItems.IndexOf(item));
        //if (tempDT.Year != refDateTime.selectedTime.Year || tempDT.Month != refDateTime.selectedTime.Month)
        //{ 
        //    return;
        //}

        refDateTime.SetDate(tempDT);
        UpdateColors();
    }
}

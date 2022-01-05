using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DatePicker : MonoBehaviour {

    public Dropdown dateDropdown, monthDropdown, yearDropDown;

    public int minYear = 1920; 
    int maxYear;

    public int date {
        get
        {
           return int.Parse(dateDropdown.options[dateDropdown.value].text);
        }
		set
		{
			for (int i = 0; i < dateDropdown.options.Count; i++) 
			{
				if (dateDropdown.options[i].text.Equals(value.ToString())) {
					dateDropdown.value = i;		
					break;
				}
			}
		}
    }
    public int month
    {
        get
        {
            return monthDropdown.value+1;
		}
		set
		{
			monthDropdown.value = Mathf.Clamp(value-1, 0, 11);		 
		}
    }
    public int year
    {
        get
        {
            return int.Parse(yearDropDown.options[yearDropDown.value].text);
		}
		set
		{
			for (int i = 0; i < yearDropDown.options.Count; i++) 
			{
				if (yearDropDown.options[i].text.Equals(value.ToString())) {
					yearDropDown.value = i;		
					break;
				}
			}
		}
    }

	void Awake()
	{
		maxYear = System.DateTime.Now.Year;
        UpdateAll();
        yearDropDown.value = yearDropDown.options.Count - 1;
        monthDropdown.value = 0;
        dateDropdown.value = 0;
		dateDropdown.onValueChanged.AddListener (OnDDValChange);
		monthDropdown.onValueChanged.AddListener (OnDDValChange);
		yearDropDown.onValueChanged.AddListener (OnDDValChange);
    }

	public void OnDDValChange(int i)
	{
		UpdateAll ();
	}

    public void UpdateAll()
    {
        UpdateYearDropdown();
        UpdateMonthDropdown();
        UpdateDateDropdown();
    }

    void UpdateYearDropdown()
    {
        List<string> temp = new List<string>();
		for (int i = maxYear; i >= minYear; i--)
        {
            temp.Add(i.ToString());
        }
        yearDropDown.ClearOptions();
        yearDropDown.AddOptions(temp);
    }

    void UpdateMonthDropdown()
    {
        List<string> temp = new List<string>();
        temp.Add("Jan");
        temp.Add("Feb");
        temp.Add("Mar");
        temp.Add("Apr");
        temp.Add("May");
        temp.Add("Jun");
        temp.Add("Jul");
        temp.Add("Aug");
        temp.Add("Sep");
        temp.Add("Oct");
        temp.Add("Nov");
        temp.Add("Dec");
        monthDropdown.ClearOptions();
        monthDropdown.AddOptions(temp);
    }

    void UpdateDateDropdown() {
        List<string> temp = new List<string>();
        for (int i = 1; i <= System.DateTime.DaysInMonth(year, month); i++)
        {
            temp.Add(i.ToString());
        }
        dateDropdown.ClearOptions();
        dateDropdown.AddOptions(temp);
    }
}

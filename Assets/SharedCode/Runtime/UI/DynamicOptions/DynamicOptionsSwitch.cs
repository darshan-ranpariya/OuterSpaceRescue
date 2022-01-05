using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOptionsSwitch : UISwitch
{  
    public DynamicOptions selection;
    public string keyword;
    public int index = -1;

    void OnEnable()
    {
        Init(); 
        selection.SelectionChanged += UpdateSwitch;
        UpdateSwitch();
    }

    void OnDisable()
    {
        if (group != null) group.RemoveSwitch(this);
        selection.SelectionChanged -= UpdateSwitch;
    }

    public void UpdateSwitch()
    {
        if (index > -1) Set(selection.selectedIndex == index);
        else Set(selection.selectedOption.key.ToLower().Contains(keyword.ToLower()));
    }

    public override void OnClick()
    {
        if (index > -1) selection.Select(index);
        else selection.Select(keyword);
    }
}
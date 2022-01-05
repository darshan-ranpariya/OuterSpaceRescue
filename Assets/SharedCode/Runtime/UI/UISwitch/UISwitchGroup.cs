using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchGroup : MonoBehaviour
{
    List<UISwitch> switches = new List<UISwitch>();
    public UISwitch defaultSwitch;

    void OnEnable()
    {
        if (defaultSwitch!=null)
        {
            defaultSwitch.Set(true);
        }
    }

    public void AddSwitch(UISwitch s)
    {
        if (!switches.Contains(s))
        {
            switches.Add(s);
        }
    }

    public void RemoveSwitch(UISwitch s)
    {
        if (switches.Contains(s))
        {
            switches.Remove(s);
        }
    }

    public void OnSwitchOn(UISwitch s)
    {
        for (int i = 0; i < switches.Count; i++)
        {
            if (s!=switches[i])
            {
                //if(switches[i].isOn)
                    switches[i].Set(false);
            }
        }
    }

    public void OnSwitchOff (UISwitch s)
    {
        for (int i = 0; i < switches.Count; i++)
        {
            if (s != switches[i])
            {
                if (switches[i].isOn) switches[i].Set(false);
            }
        }
    }

    public UISwitch GetSelectedSwitch()
    {
        for (int i = 0; i < switches.Count; i++)
        {
            if (switches[i].isOn)
            {
                return switches[i];
            }
        }
        return defaultSwitch;
    }
}

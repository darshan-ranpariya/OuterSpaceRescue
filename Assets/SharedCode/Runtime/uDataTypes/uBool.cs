using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class uBool : uVar<bool, uBoolComponent>
{
    public GameObject[] onTrueObjects = new GameObject[] { };
    public GameObject[] onFalseObjects = new GameObject[] { };
    public Panel[] panels = new Panel[] { };
    public UISwitch[] switches = new UISwitch[] { };
    public override void OnValueChanged()
    {
        if (onTrueObjects!=null)
        { 
            for (int i = 0; i < onTrueObjects.Length; i++)
            {
                if (onTrueObjects[i] != null) onTrueObjects[i].SetActive(Value);
            }
        }
        if (onFalseObjects != null)
        {
            for (int i = 0; i < onFalseObjects.Length; i++)
            {
                if (onFalseObjects[i] != null) onFalseObjects[i].SetActive(!Value);
            }
        }
        if (panels != null)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] != null)
                {
                    if(Value) panels[i].Activate();
                    else panels[i].Deactivate();
                }
            }
        }
        if (switches != null)
        {
            for (int i = 0; i < switches.Length; i++)
            {
                if (switches[i] != null)
                {
                    switches[i].Set(Value);
                }
            }
        }
    }
}

public abstract class uBoolComponent : MonoBehaviour, IuVarHandler<bool>
{
    public abstract void Handle(ref bool s);
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(uBool))]
public class uBoolDrawer : uVarDrawer
{
    string[] arr = new string[] { "onTrueObjects", "onFalseObjects", "panels", "switches", "components" };
    public override string[] arrays
    {
        get
        {
            return arr;
        }
    }
}
#endif
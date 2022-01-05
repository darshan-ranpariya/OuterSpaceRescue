using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class uColor : uVar<Color, uColorComponent>
{
    public Graphic[] graphics = new Graphic[] { }; 

    public override void OnValueChanged()
    {
        if (graphics != null)
        {
            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics[i] != null) graphics[i].color = m_value;
            }
        } 
    }
}

public abstract class uColorComponent : MonoBehaviour, IuVarHandler<Color>
{
    public abstract void Handle(ref Color c);
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(uColor))]
public class uColorDrawer : uVarDrawer
{
    string[] arr = new string[] { "graphics", "components" };
    public override string[] arrays
    {
        get
        {
            return arr;
        }
    }
}
#endif
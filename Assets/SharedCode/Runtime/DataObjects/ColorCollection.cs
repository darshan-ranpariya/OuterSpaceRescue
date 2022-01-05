using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ColorCollection", menuName = "ColorCollection")]
public class ColorCollection : ScriptableObject 
{
    [Serializable]
    public class ColorItem
    {
        public string name;
        public Color color;
    }
    public List<ColorItem> colors = new List<ColorItem>();
    public Color fallbackColor = Color.white;

    public Color GetColor(int _index)
    {
        try
        {
            return colors[_index].color;
        }
        catch
        {
            return fallbackColor;
        }
    }

    public Color GetColor(string _name, System.StringComparison _comparison = System.StringComparison.CurrentCultureIgnoreCase)
    {
        try
        {
            return colors.Find((s) => { return s.name.Equals(_name, _comparison); }).color;
        }
        catch
        {
            return fallbackColor;
        }
    }
} 
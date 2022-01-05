using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor; 
#endif


[System.Serializable]
public class uNumber : uVar<double, uNumberComponent>
{ 
    public Text[] textComps = new Text[] { };
    public TMPro.TextMeshProUGUI[] textProComps = new TMPro.TextMeshProUGUI[] { };
    public UINumberText[] numTextComps = new UINumberText[] { };
     
    public bool formatWithCommas = false;
    public bool formatWithK = false;
    public bool displayEmptyIfZero = false;
    public bool displayEmptyIfNegative = false;

    public string prefix = "";
    public string suffix = "";

    public string format = "";

    public int ValueAsInt
    {
        get { return (int)Value; }
    }

    Coroutine cr;
    public void InterpolateValue(double val, double from, float duration)
    {
        m_value = val;
        if (cr != null) CommonMono.instance.StopCoroutine(cr);
        cr = CommonMono.instance.StartCoroutine(Interpolation(from, duration));
    }
    IEnumerator Interpolation(double from, float duration)
    {
        float t = 0;
        float deltaTime = 0, realTimeLastFrame = Time.realtimeSinceStartup;
        while (t < duration)
        {
            deltaTime = Time.realtimeSinceStartup - realTimeLastFrame;
            realTimeLastFrame = Time.realtimeSinceStartup;
            t += deltaTime;
            SetComps(lerpDouble(from, m_value, (t / duration * 1f)));
            yield return null;
        }
        SetComps(m_value);
        cr = null;
    }
    double lerpDouble(double i, double f, float t)
    {
        return i + ((f - i) * t);
    }

    public override void OnValueChanged()
    {
        SetComps(m_value);
    }

    void SetComps(double tempVal)
    {
        if (textComps != null)
        {
            for (int i = 0; i < textComps.Length; i++)
            {
                if (textComps[i] != null)
                {
                    if (displayEmptyIfZero && tempVal == 0) textComps[i].text = NumToStr(tempVal);
                    if (displayEmptyIfNegative && tempVal < 0) textComps[i].text = NumToStr(tempVal);
                    else textComps[i].text = NumToStr(tempVal);
                }
            }
        }

        if (textProComps != null)
        {
            for (int i = 0; i < textProComps.Length; i++)
            {
                if (textProComps[i] != null)
                {
                    if (displayEmptyIfZero && tempVal == 0) textProComps[i].text = NumToStr(tempVal);
                    else if (displayEmptyIfNegative && tempVal < 0) textProComps[i].text = NumToStr(tempVal);
                    else textProComps[i].text = NumToStr(tempVal);
                }
            }
        }

        if (numTextComps != null)
        {
            for (int i = 0; i < numTextComps.Length; i++)
            {
                if (numTextComps[i] != null)
                {
                    numTextComps[i].number = tempVal;
                }
            }
        }
    }

    string NumToStr(double n)
    {
        if (!string.IsNullOrEmpty(format)) return n.ToString(format);
        if (displayEmptyIfZero && n == 0) return string.Empty;
        if (displayEmptyIfNegative && n < 0) return string.Empty;
        return prefix + FormattedString.FromDouble(n, formatWithCommas, formatWithK) + suffix;
    }
}


public abstract class uNumberComponent : MonoBehaviour, IuVarHandler<double>
{
    public abstract void Handle(ref double s);
}

public static class uNumberExt
{
    public static double GetSum(this uNumber[] arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        double sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            sum += arr[i].Value;
        }
        return sum;
    }
}

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(uNumber))]
public class uNumberDrawer : uVarDrawer
{
    string[] arr = new string[] { "textComps", "textProComps", "components" };
    public override string[] arrays
    {
        get
        {
            return arr;
        }
    }

    protected override int GetUnFoldedContentLines()
    {
        return 2;
    } 

    //public override void DrawValueContent(Rect position, SerializedProperty property, GUIContent label)
    //{
    //Rect r = GetCurrentRect();
    //Rect r0 = GetLabelArea(r);
    //Rect r1 = GetValueArea(r);
    //Rect r1_1 = GetColumnedRect(r1, 5, 0);
    //Rect r1_2 = GetColumnedRect(r1, 5, 1, 3);
    //Rect r1_3 = GetColumnedRect(r1, 5, 4);

    //EditorGUI.PropertyField(r1_1, property.FindPropertyRelative("prefix"), GUIContent.none);
    //EditorGUI.PropertyField(r1_2, property.FindPropertyRelative("m_value"), GUIContent.none);
    //EditorGUI.PropertyField(r1_3, property.FindPropertyRelative("suffix"), GUIContent.none);
    //}

    public override void DrawFoldoutContent(Rect position, SerializedProperty property, GUIContent label)
    {
        spacing++;

        Rect r = GetNextRect();
        Rect r0 = GetLabelArea(r);
        Rect r1 = GetValueArea(r);
        Rect r1_1 = GetColumnedRect(r1, 10, 0, 1);
        Rect r1_2 = GetColumnedRect(r1, 10, 3, 4);
        Rect r1_3 = GetColumnedRect(r1, 10, 6);
        Rect r1_4 = GetColumnedRect(r1, 10, 7);
        Rect r1_5 = GetColumnedRect(r1, 10, 8);
        Rect r1_6 = GetColumnedRect(r1, 10, 9);
        Rect r2 = GetValueArea(GetNextRect());

        GUI.Label(r, new GUIContent("Formatting", "Prefix, Suffix, Use Comma, Use K, Empty if 0, Empty if Negative"));
        EditorGUI.PropertyField(r1_1, property.FindPropertyRelative("prefix"), GUIContent.none);
        EditorGUI.PropertyField(r1_2, property.FindPropertyRelative("suffix"), GUIContent.none);
        EditorGUI.PropertyField(r1_3, property.FindPropertyRelative("formatWithCommas"), GUIContent.none);
        EditorGUI.PropertyField(r1_4, property.FindPropertyRelative("formatWithK"), GUIContent.none);
        EditorGUI.PropertyField(r1_5, property.FindPropertyRelative("displayEmptyIfZero"), GUIContent.none);
        EditorGUI.PropertyField(r1_6, property.FindPropertyRelative("displayEmptyIfNegative"), GUIContent.none);
        EditorGUI.PropertyField(r2, property.FindPropertyRelative("format"), GUIContent.none);
    }
} 
#endif
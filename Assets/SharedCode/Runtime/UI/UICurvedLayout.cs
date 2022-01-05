using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurvedLayout : LayoutGroup
{
    public RectTransform refObject;
    public bool resize = true;
    public float thickness = 10;

    public float radius = 100;
    public int segments; 

    public override void CalculateLayoutInputVertical()
    { 
    }

    public override void SetLayoutHorizontal()
    { 
    }

    public override void SetLayoutVertical()
    {
        if (refObject != null && !Application.isPlaying) return;

        int s = segments;

        #region RectGeneration
        if (refObject == null) return;
        if (radius == 0) s = 0;
        if (segments < 0) s = 0;

        for (int i = 0; i < s || i < transform.childCount; i++)
        {
            if (i < s)
            {
                if (i < transform.childCount)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    Instantiate(refObject, transform, true);
                }
            }
            else if (i < transform.childCount)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        #endregion 

        float w = (2 * Mathf.PI * radius) / s;

        for (int i = 0; i < s; i++)
        {
            RectTransform crt = (RectTransform)transform.GetChild(i);
            if (resize) crt.sizeDelta = new Vector2(w, thickness);
            float a = (360f * i) / s;
            crt.localPosition = new Vector3(radius * Mathf.Cos(a * Mathf.Deg2Rad), radius * Mathf.Sin(a * Mathf.Deg2Rad), 0);
            crt.localEulerAngles = new Vector3(0, 0, a+90);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICarousel : MonoBehaviour
{
    RectTransform _thisRectTransform;
    RectTransform thisRectTransform
    {
        get
        {
            if (_thisRectTransform == null) _thisRectTransform = GetComponent<RectTransform>();
            return _thisRectTransform;
        }
    }

    public Transform[] items;
    public int selectItemOnAwake = -1;
    public int selectedItemIndex;
    public float selectedItemScale = 1;
    public float animationDuration = 0.3f;
    public AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
    public event Action selectionChanged;

    UICarouselExtBase[] _exts;
    bool extsGet = false;
    UICarouselExtBase[] exts
    {
        get
        {
            if (!extsGet)
            {
                _exts = GetComponents<UICarouselExtBase>();
                extsGet = true;
            }
            return _exts;
        }
    }

    void Awake()
    {
        if (selectItemOnAwake >= 0)
        {
            SelectItem(selectItemOnAwake);
        }
    }

    void OnEnable()
    { 

    }

    Interpolate.Position[] posAnims;
    Interpolate.Scale[] scaleAnims;
    Vector3[] positions;
    float startX;
    float xDistance;
    public void SelectItem(int indx)
    { 
        if (posAnims != null)
        {
            for (int i = 0; i < posAnims.Length; i++)
            {
                if (posAnims[i] != null)
                {
                    posAnims[i].Stop();
                }
            }
        }
        if (scaleAnims != null)
        {
            for (int i = 0; i < scaleAnims.Length; i++)
            {
                if (scaleAnims[i] != null)
                {
                    scaleAnims[i].Stop();
                }
            }
        }

        xDistance = (thisRectTransform.rect.xMax - thisRectTransform.rect.xMin)/items.Length;
        startX = (xDistance / 2) + thisRectTransform.rect.xMin;

        positions = new Vector3[items.Length];
        posAnims = new Interpolate.Position[items.Length];
        scaleAnims = new Interpolate.Scale[items.Length];

        if (indx >= items.Length) indx = indx % items.Length;
        else if (indx < 0) indx = items.Length + (indx % items.Length);
        selectedItemIndex = indx; 

        for (int i = 0; i < items.Length; i++)
        {
            positions[i] = new Vector3(startX + xDistance * ((selectedItemIndex + items.Length - i + (int)(items.Length/2)) % items.Length), 0, 0);
        }

        for (int i = 0; i < items.Length; i++)
        {
            posAnims[i] = new Interpolate.Position(
                items[i],
                items[i].localPosition,
                positions[i],
                animationDuration,
                true,
                animationCurve
                );

            scaleAnims[i] = new Interpolate.Scale(
                items[i],
                items[i].localScale,
                Vector3.one * (i == selectedItemIndex).ToFloat(selectedItemScale, 1),
                animationDuration
                );

            if (i == selectedItemIndex)
            {
                items[i].SetAsLastSibling();
            }
        }

        if (exts!=null)
        {
            for (int i = 0; i < exts.Length; i++)
            {
                exts[i].OnSelectionChanged();
            }
        }

        if (selectionChanged!=null) selectionChanged();
    }
    public void SelectItem(Transform item)
    {
        int s = items.IndexOf(item);
        if (s >= 0) SelectItem(s);
    } 
    public void SelectNextItem()
    { 
        SelectItem(selectedItemIndex + 1);
    }
    public void SelectPrevItem()
    { 
        SelectItem(selectedItemIndex - 1);
    }


    [Header("Test")]
    public int testSelect;
    public void Test()
    {
        SelectItem(testSelect);
    }  
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Image))]
public class LayoutImageElement : MonoBehaviour, ILayoutElement
{

    Image _img;
    Image img
    {
        get
        {
            if (_img == null)
            {
                _img = GetComponent<Image>();
            }
            return _img;
        }
    }

    RectTransform _rt;
    RectTransform rt
    {
        get
        {
            if (_rt == null)
            {
                _rt = GetComponent<RectTransform>();
            }
            return _rt;
        }
    }

    RectTransform _parentRt;
    RectTransform parentRt
    {
        get
        {
            if (_parentRt == null)
            {
                _parentRt = rt.parent.GetComponent<RectTransform>();
            }
            return _parentRt;
        }
    }

    ILayoutGroup _group;
    ILayoutGroup group
    {
        get
        {
            if (_group == null)
            {
                _group = rt.parent.GetComponent<ILayoutGroup>();
            }
            return _group;
        }
    }

    public float minWidth
    {
        get
        {
            return 0;
        }
    }

    public float minHeight
    {
        get
        {
            return 0;
        }
    }

    public float m_preferredWidth;
    public float preferredWidth
    {
        get
        {
            return m_preferredWidth;
        }
    }

    public float m_preferredHeight;
    public float preferredHeight
    {
        get
        {
            return m_preferredHeight;
        }
    }

    float m_flexibleWidth;
    public float flexibleWidth
    {
        get
        {
            return m_flexibleWidth;
        }
    }

    float m_flexibleHeight;
    public float flexibleHeight
    {
        get
        {
            return m_flexibleHeight;
        }
    }

    public int layoutPriority
    {
        get
        {
            return 0;
        }
    }

    public void CalculateLayoutInputHorizontal()
    {
        //if (autoWidth)
        //{ 
        //    m_preferredWidth = rt.rect.height * ar;
        //}
        //else m_preferredWidth = rt.rect.width;
    }

    public void CalculateLayoutInputVertical()
    {
        //if (autoHeight)
        //{ 
        //    m_preferredHeight =  (parentRt.rect.width - group. ) / ar;
        //}
        //else m_preferredHeight = rt.rect.height;
    }

    public bool autoWidth;
    public bool autoHeight;

    float ar
    {
        get
        {
            return (float)img.sprite.texture.width / (float)img.sprite.texture.height;
        }
    }
}
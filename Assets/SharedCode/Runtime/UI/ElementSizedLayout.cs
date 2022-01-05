using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ElementSizedLayout : LayoutGroup
{
    [TypeConstraint(typeof(ILayoutElement))]
    public RectTransform element;
    public ILayoutElement elementComp;

    public float m_minWidth = 0;
    public override float minWidth
    {
        get
        {
            return m_minWidth;
        }
    }

    public float m_minHeight = 0;
    public override float minHeight
    {
        get
        {
            return m_minHeight;
        }
    }

    public float m_maxWidth = 0;
    public float m_maxHeight = 0;

    float _preferredHeight = 0;
    public override float preferredHeight
    {
        get
        {
            return _preferredHeight;
        }
    }

    float _preferredWidth = 0;
    public override float preferredWidth
    {
        get
        {
            return _preferredWidth;
        }
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (elementComp == null) elementComp = element.GetComponent<ILayoutElement>();
        if (elementComp != null)
        {
            float w = elementComp.preferredWidth;
            if (m_maxWidth > 0 && w > m_maxWidth)
            {
                _preferredWidth = m_maxWidth;
            }
            else
            {
                _preferredWidth = w;
            }
        }
        else _preferredWidth = base.preferredWidth;
    }

    public override void CalculateLayoutInputVertical()
    {
        if (elementComp == null) elementComp = element.GetComponent<ILayoutElement>();
        if (elementComp != null)
        {
            float h = elementComp.preferredHeight;
            if (m_maxHeight > 0 && h > m_maxHeight)
            {
                _preferredHeight = m_maxHeight;
            }
            else
            {
                _preferredHeight = h;
            }
        }
        else _preferredHeight = base.preferredHeight;
    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }

    public void SD()
    {
        SetDirty();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ElementSizedLayout))]
public class ElementSizedLayoutEditor : Editor
{
    public ElementSizedLayout script;
    void OnEnable()
    {
        script = target as ElementSizedLayout;
    }
    public override void OnInspectorGUI()
    {
        script.element = (RectTransform)EditorGUILayout.ObjectField("Element", script.element, typeof(RectTransform), true);
        script.m_minWidth = EditorGUILayout.FloatField("Min Width", script.m_minWidth);
        script.m_minHeight = EditorGUILayout.FloatField("Min Height", script.m_minHeight);
        script.m_maxWidth = EditorGUILayout.FloatField("Max Width", script.m_maxWidth);
        script.m_maxHeight = EditorGUILayout.FloatField("Max Height", script.m_maxHeight);

        if (GUI.changed)
        {
            script.SD();
        }
    }
}
#endif
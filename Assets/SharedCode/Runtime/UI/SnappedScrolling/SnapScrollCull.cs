using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapScrollCull : MonoBehaviour
{
    public SnapScroll scrollView;
    public TransformEvents objectsParentEvents;
    public RectTransform[] objectRects;

    void OnEnable()
    {
        scrollView.Scrolled += ScrollView_Scrolled;

        if (objectsParentEvents == null) objectsParentEvents = scrollView.objectsParentRect.GetComponent<TransformEvents>();
        if (objectsParentEvents == null) objectsParentEvents = scrollView.objectsParentRect.gameObject.AddComponent<TransformEvents>();
        objectsParentEvents.TransformChildrenChanged += UpdateChildren;

        UpdateChildren();
    }

    void OnDisable()
    {
        scrollView.Scrolled -= ScrollView_Scrolled;
        objectsParentEvents.TransformChildrenChanged -= UpdateChildren;
    }

    void UpdateChildren()
    {
        objectRects = new RectTransform[scrollView.objectsParentRect.childCount];
        for (int i = 0; i < objectRects.Length; i++)
        {
            objectRects[i] = (RectTransform)scrollView.objectsParentRect.GetChild(i);
        }
    }
     
    void ScrollView_Scrolled()
    {
        for (int i = 0; i < objectRects.Length; i++)
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TransformEvents : MonoBehaviour
{
    public event System.Action TransformParentChanged;
    public event System.Action TransformChildrenChanged;
    public UnityEvent TransformParentChangedEvent;
    public UnityEvent TransformChildrenChangedEvent;

    public uNumber childCountText;

    void OnTransformParentChanged()
    {
        TransformParentChangedEvent.Invoke();
        if(TransformParentChanged != null) TransformParentChanged();
    }

    void OnTransformChildrenChanged()
    {
        TransformChildrenChangedEvent.Invoke();
        if (TransformChildrenChanged != null) TransformChildrenChanged();

        //int c = 0;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    if (transform.GetChild(i).gameObject.activeSelf)
        //    {
        //        c++;
        //    }
        //}
        //if (childCountText != null) childCountText.value = c;
        if (childCountText != null) childCountText.Value = transform.childCount;
    }
}

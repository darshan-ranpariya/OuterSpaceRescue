using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICullObject
{
    RectTransform rectT { get; }
    void Cull(bool cull);
}

public class CullObject : MonoBehaviour, ICullObject
{
    RectTransform _rectT;
    public RectTransform rectT
    {
        get
        {
            if (_rectT == null) _rectT = (RectTransform)transform;
            return _rectT;
        }
    }

    public void Cull(bool cull)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(cull);
        }
    }
}

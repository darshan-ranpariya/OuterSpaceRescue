using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectCull : MonoBehaviour
{
    class ChildData
    {
        bool culled;
        public RectTransform rect;
        //public Behaviour[] comps;
        public RectTransform[] children;

        byte lastStatus = 0;
        const byte lsa = 1;
        const byte lsn = 2;

        public void Cull(bool active)
        {
            if (active)
            {
                if (lastStatus == lsa) return;
            }
            else
            {
                if (lastStatus == lsn) return;
            }
            lastStatus = active ? lsa : lsn;

            //foreach (var item in comps) item.enabled = active;
            foreach (var item in children) item.gameObject.SetActive(active);
        }
    }

    public RectTransform boundingRect;
    Dictionary<RectTransform, ChildData> children = new Dictionary<RectTransform, ChildData>();
     
    Vector3 lastPos;  

    void OnTransformChildrenChanged()
    {
        UpdateChildren();
        Cull();
    }

    void Update()
    {
        if (!lastPos.Equals(transform.position))
        {
            Cull();
        } 
    }

    void UpdateChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rt = (RectTransform)transform.GetChild(i);
            if (!children.ContainsKey(rt))
            {
                children.Add(rt, new ChildData()
                {
                    rect = rt,
                    //comps = (Behaviour[])rt.GetComponents(typeof(Behaviour)),
                    children = rt.GetAllChildren(),
                });
            }
        }

        List<RectTransform> destroyedRects = new List<RectTransform>();
        foreach (var item in children)
        {
            if (item.Key == null)
            {
                destroyedRects.Add(item.Key);
            }
        }
        for (int i = 0; i < destroyedRects.Count; i++)
        {
            children.Remove(destroyedRects[i]);
        }
        destroyedRects.Clear();
    }

    void Cull()
    { 
        foreach (var item in children)
        {
            item.Value.Cull(CheckRectIntersect(ref boundingRect, ref item.Value.rect));
        }
        lastPos = transform.position;
    }

    bool CheckRectIntersect(ref RectTransform a, ref RectTransform b)
    { 
        return !(
            b.position.y + b.rect.yMax * b.lossyScale.y < a.position.y + a.rect.yMin * a.lossyScale.y
            || b.position.y + b.rect.yMin * b.lossyScale.y > a.position.y + a.rect.yMax * a.lossyScale.y
            || b.position.x + b.rect.xMin * b.lossyScale.x > a.position.x + a.rect.xMax * a.lossyScale.x
            || b.position.x + b.rect.xMax * b.lossyScale.x < a.position.x + a.rect.xMin * a.lossyScale.x
            ); 
    }
}

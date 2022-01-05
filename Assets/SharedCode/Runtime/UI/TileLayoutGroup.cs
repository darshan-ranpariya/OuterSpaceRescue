using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("Layout/Tile Layout Group", 152)]
public class TileLayoutGroup : LayoutGroup
{

    [SerializeField]
    protected RectTransform m_refRect = null;
    public RectTransform refRect { get { return m_refRect; } set { SetProperty(ref m_refRect, value); } }

    [SerializeField]
    protected Vector2 m_refSize = Vector2.one * 100;
    public Vector2 refSize { get { return m_refSize; } set { SetProperty(ref m_refSize, value); } }

    [SerializeField]
    protected Vector2[] m_tileSizes;
    public Vector2[] tileSizes { get { return m_tileSizes; } set { SetProperty(ref m_tileSizes, value); } }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (refRect == null) refRect = (RectTransform)transform.parent;
        refRect = refRect;
    }
#endif

    public float pw = 1;
    public override float preferredWidth
    {
        get
        {
            return pw;
        }
    }

    public float ph = 1;
    public override float preferredHeight
    {
        get
        {
            return ph;
        }
    }


    Vector2[] pa = null;
    Vector2[] sa = null;
    List<RectTransform> childrenList = new List<RectTransform>();
    bool iclp = false;
    List<RectTransform> ignoredChildrenList = new List<RectTransform>();
    public override void CalculateLayoutInputVertical()
    {
        if (!iclp)
        {
            iclp = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                LayoutElement le = transform.GetChild(i).GetComponent<LayoutElement>();
                if (le != null && le.ignoreLayout)
                {
                    ignoredChildrenList.Add((RectTransform)(transform.GetChild(i)));
                }
            }
        }
        childrenList.Clear();
        RectTransform rt = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            rt = (RectTransform)(transform.GetChild(i));
            if (rt.gameObject.activeSelf && !ignoredChildrenList.Contains(rt))
            {
                childrenList.Add(rt);
            }
        }

        pa = new Vector2[childrenList.Count];
        sa = new Vector2[childrenList.Count];

        Vector2 s = Vector2.zero;
        Vector2 o = Vector2.zero;
        float mx = ((RectTransform)transform).rect.width;
        float my = 0;
        for (int i = 0; i < childrenList.Count; i++)
        {
            int childIndex = childrenList[i].GetSiblingIndex();

            s = refSize;
            if (i < tileSizes.Length) s = tileSizes[childIndex];
            if (s.x < 0) s.x = refRect.rect.width * s.x * -1;
            else if (s.x == 0) s.x = refSize.x;
            if (s.y < 0) s.y = refRect.rect.height * s.y * -1;
            else if (s.y == 0) s.y = refSize.y;

            if (o.x + s.x > mx)
            {
                o.x = 0;
                o.y += my;
            }
            
            pa[i] = new Vector2(o.x, o.y);
            sa[i] = new Vector2(s.x, s.y);

            if (s.y > my) my = s.y;
            o.x += s.x;

            if (i == childrenList.Count - 1)
            {   
                pw = o.x + s.x;
                ph = o.y + s.y;
            }
        }
    }

    public override void SetLayoutHorizontal()
    { 
    }

    public override void SetLayoutVertical()
    {  
        for (int i = 0; i < childrenList.Count; i++)
        {
            SetChildAlongAxis(childrenList[i], 0, pa[i].x, sa[i].x);
            SetChildAlongAxis(childrenList[i], 1, pa[i].y, sa[i].y);
        }

        //RectTransform rt = (RectTransform)transform;
        //Vector2 s = Vector2.zero;
        //Vector2 o = Vector2.zero;
        //float mx = rt.rect.width;
        //float my = 0;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    s = refSize;
        //    if (i < tileSizes.Length) s = tileSizes[i];
        //    if (s.x < 0) s.x = refRect.rect.width * s.x * -1; 
        //    else if (s.x == 0) s.x = refSize.x;
        //    if (s.y < 0) s.y = refRect.rect.height * s.y * -1;
        //    else if (s.y == 0) s.y = refSize.y;

        //    if (o.x + s.x > mx)
        //    {
        //        o.x = 0;
        //        o.y += my;
        //    }

        //    rt = (RectTransform)transform.GetChild(i);
        //    SetChildAlongAxis((RectTransform)transform.GetChild(i), 0, o.x, s.x);
        //    SetChildAlongAxis((RectTransform)transform.GetChild(i), 1, o.y, s.y);

        //    if (s.y > my) my = s.y;
        //    o.x += s.x;
        //}
    }
}

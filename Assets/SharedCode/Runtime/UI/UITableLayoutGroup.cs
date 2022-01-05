using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UITableLayoutGroup : UIBehaviour, ILayoutGroup 
{
    public enum Direction { Horizontal, Vertical }

    RectTransform _rt;
    RectTransform rt
    {
        get
        {
            if (_rt == null) _rt = GetComponent<RectTransform>();
            return _rt;
        }
    }

    bool childrenModified = true;
    RectTransform[] _children;
    public RectTransform[] children
    {
        get
        {
            if (childrenModified || _children == null)
            {
                List<int> l = new List<int>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).gameObject.activeSelf) l.Add(i);
                }

                _children = new RectTransform[l.Count];
                for (int i = 0; i < l.Count; i++)
                {
                    _children[i] = (RectTransform)transform.GetChild(l[i]);
                }
            }
            return _children;
        }
    }
         
    [SerializeField]
    protected RectTransform m_viewPort = null;
    public RectTransform viewPort { get { return m_viewPort; } set { SetProperty(ref m_viewPort, value); } }

    [SerializeField]
    protected Vector2 m_elementSize = Vector2.zero;
    public Vector2 elementSize { get { return m_elementSize; } set { SetProperty(ref m_elementSize, value); } }
    public Vector2 elSize;

    [SerializeField]
    protected Direction m_direction = Direction.Horizontal;
    public Direction direction { get { return m_direction; } set { SetProperty(ref m_direction, value); } }

    public event System.Action LayoutUpdated;

    float w, h; 
    float wm, hm;
    internal int cc, rc; 

    public void SetLayoutHorizontal()
    {
        if (viewPort == null) return;
        if (elementSize.x <= 0 || elementSize.y <= 0) return;

        w = viewPort.rect.xMax - viewPort.rect.xMin; 
        cc = Mathf.Clamp(Mathf.FloorToInt(w / elementSize.x), 1, 100);
        //wm = (w % elementSize.x) / Mathf.Clamp(cc - 1, 1, w);
        wm = 0;
    }

    public void SetLayoutVertical()
    {
        if (viewPort == null) return;
        if (elementSize.x <= 0 || elementSize.y <= 0) return;

        h = viewPort.rect.yMax - viewPort.rect.yMin;
        rc = Mathf.Clamp(Mathf.FloorToInt(h / elementSize.y), 1, 100);
        //hm = (h % elementSize.y) / Mathf.Clamp(rc, 1, h);
        hm = 0;

        //if (cc * rc > children.Length)
        //{
        //    cc = Mathf.Clamp(children.Length / rc, 1, cc);
        //    rc = Mathf.Clamp(children.Length / cc, 1, cc);
        //}
        elSize = new Vector2(w/cc, h/rc);

        if (direction == Direction.Horizontal)
        {
            int tcc = Mathf.CeilToInt(children.Length * 1f / rc);
            rt.sizeDelta = new Vector2(elSize.x * tcc + wm * (tcc - 1), h);
        }
        else
        {
            int trc = Mathf.CeilToInt(children.Length * 1f / cc);
            rt.sizeDelta = new Vector2(w, elSize.y * trc + hm * (trc - 1));
        }

        for (int i = 0; i < children.Length; i++)
        {
            int r = 0, c = 0;
            if (direction == Direction.Horizontal)
            {
                c = (i / rc);
                r = (i % rc);
            }
            else
            {
                c = (i % cc);
                r = (i / cc);
            }
            children[i].anchoredPosition = new Vector3(
                c * elSize.x - children[i].rect.xMin + (c * wm),
                -(r * elSize.y + children[i].rect.yMax + (r * hm)),
                0);
            children[i].sizeDelta = elSize;
        }
        lup = true;
    }

    protected void SetProperty<T>(ref T currentValue, T newValue)
    {
        if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue))) return;
        currentValue = newValue;
        SetDirty();
    }

    protected void SetDirty()
    {
        if (!IsActive()) return;
        if (CanvasUpdateRegistry.IsRebuildingLayout()) return;
        LayoutRebuilder.MarkLayoutForRebuild(rt);

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirty();
    } 
    protected override void OnDisable()
    { 
        LayoutRebuilder.MarkLayoutForRebuild(rt);
        base.OnDisable(); 
    }
    float lvx, lvy;
    private bool lup;
    float llup;
    void Update()
    {
        if ((viewPort.rect.xMax - viewPort.rect.xMin) != lvx || (viewPort.rect.yMax - viewPort.rect.yMin) != lvy)
        {
            SetDirty();
        }
        lvx = (viewPort.rect.xMax - viewPort.rect.xMin);
        lvy = (viewPort.rect.yMax - viewPort.rect.yMin);
        if (lup && Time.realtimeSinceStartup - llup > 1)
        {
            llup = Time.realtimeSinceStartup;
            if (LayoutUpdated != null) LayoutUpdated();
            lup = false;
        }
    }

    void OnTransformChildrenChanged()
    {
        SetDirty();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        SetDirty();
    }
#endif
}

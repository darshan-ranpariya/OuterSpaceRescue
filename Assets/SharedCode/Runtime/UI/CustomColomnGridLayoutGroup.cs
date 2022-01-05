using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CustomColomnGridLayoutGroup : LayoutGroup
{
 
    [SerializeField]
    protected RectTransform m_refRect = null;
    public RectTransform refRect { get { return m_refRect; } set { SetProperty(ref m_refRect, value); } }

    public Vector2 refSize;

    [SerializeField]
    protected int[] m_objectsPerRow = new int[0];
    public int[] objectsPerRow { get { return m_objectsPerRow; } set { SetProperty(ref m_objectsPerRow, value); } }


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (refRect == null) refRect = (RectTransform)transform.parent;
        refRect = refRect;
    }
#endif

    float pw = 1;
    public override float preferredWidth
    {
        get
        {
            return pw;
        }
    }

    float ph = 1;
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
    public override void CalculateLayoutInputVertical()
    {
        List<List<int>> rows = new List<List<int>>();
        List<int> buttonsRow = new List<int>();
        List<int> buttonsColumn = new List<int>();
        for (int i = 0; i < objectsPerRow.Length; i++)
        {
            for (int j = 0; j < objectsPerRow[i]; j++)
            {
                buttonsRow.Add(i);
                buttonsColumn.Add(j);
            }
        }
        childrenList.Clear();
        RectTransform rt = null;
        int lastEnabledRow = -1;
        int currentRow = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            rt = (RectTransform)(transform.GetChild(i));
            if (rt.gameObject.activeSelf)
            {
                childrenList.Add(rt);
                if (i >= buttonsRow.Count) continue;
                int definedRow = buttonsRow[i];
                if (lastEnabledRow == -1) currentRow = 0;
                else if (lastEnabledRow != definedRow) currentRow++;
                lastEnabledRow = definedRow;
                if (rows.Count <= currentRow) rows.Add(new List<int>());
                rows[currentRow].Add(i);
            }
        }

        pw = ((RectTransform)rectTransform.parent).rect.width;
        ph = ((RectTransform)rectTransform.parent).rect.height;
        pa = new Vector2[childrenList.Count];
        sa = new Vector2[childrenList.Count];

        int tr = rows.Count;
        int tc = 0;
        int ci = 0;
        for (int y = 0; y < tr; y++)
        {
            tc = rows[y].Count;
            for (int x = 0; x < tc; x++)
            {
                sa[ci] = refSize;
                switch (childAlignment)
                {
                    case TextAnchor.UpperLeft:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.UpperCenter:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.UpperRight:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.MiddleLeft:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.MiddleCenter:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.MiddleRight:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.LowerLeft:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.LowerCenter:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                    case TextAnchor.LowerRight:
                        pa[ci] = new Vector2(x * sa[ci].x, y * sa[ci].y);
                        break;
                }
                ci++;
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
    }
}


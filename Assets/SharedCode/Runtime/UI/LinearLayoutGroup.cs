using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class LinearLayoutGroup : UIBehaviour, ILayoutElement, ILayoutGroup
    {
        [System.NonSerialized]
        private RectTransform m_Rect;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        float m_minWidth; 

        [SerializeField] 
        private float m_maxWidth;
        public float maxWidth { get { return m_maxWidth; } set { m_maxWidth = value; SetDirty(); } }

        [SerializeField] 
        private TextAnchor m_childAlignment = TextAnchor.UpperLeft; 
        public TextAnchor childAlignment { get { return m_childAlignment; } set { m_childAlignment = value; SetDirty(); } }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
#endif

        protected virtual void OnTransformChildrenChanged()
        {
            SetDirty();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            if (!CanvasUpdateRegistry.IsRebuildingLayout())
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            else
                StartCoroutine(DelayedSetDirty(rectTransform));
        } 

        IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        #region ILayoutElement 
        float layoutWidth;
        float layoutHeight;

        public float minWidth
        {
            get
            {
                return m_minWidth;
            }
        }

        public float preferredWidth
        {
            get
            {
                return layoutWidth;
            }
        }

        public float flexibleWidth
        {
            get
            {
                return maxWidth == 0 ? 1 : -1;
            }
        }

        public float minHeight
        {
            get
            {
                return -1;
            }
        }

        public float preferredHeight
        {
            get
            {
                return layoutHeight;
            }
        }

        public float flexibleHeight
        {
            get
            {
                return -1;
            }
        }

        public int layoutPriority
        {
            get
            {
                return 0;
            }
        }

        List<RectTransform> m_RectChildren = new List<RectTransform>(); 
        List<Component> toIgnoreList = new List<Component>();
        void UpdateChildrenList()
        {
            m_RectChildren.Clear();
            toIgnoreList.Clear();
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                var rect = rectTransform.GetChild(i) as RectTransform;
                if (rect == null || !rect.gameObject.activeInHierarchy)
                    continue;

                rect.GetComponents(typeof(ILayoutIgnorer), toIgnoreList);

                if (toIgnoreList.Count == 0)
                {
                    m_RectChildren.Add(rect);
                    continue;
                }

                for (int j = 0; j < toIgnoreList.Count; j++)
                {
                    var ignorer = (ILayoutIgnorer)toIgnoreList[j];
                    if (!ignorer.ignoreLayout)
                    {
                        m_RectChildren.Add(rect);
                        break;
                    }
                }
            }
        }

        List<Vector2> rowsSize = new List<Vector2>(); 
        List<int> childrenRow = new List<int>();
        List<float> childrenX = new List<float>();
        List<float> childrenY = new List<float>();
        List<Vector2> childrenSize = new List<Vector2>();
        public void CalculateLayoutInputHorizontal()
        {
            UpdateChildrenList();

            childrenRow.Clear();
            childrenX.Clear();
            childrenY.Clear();
            childrenSize.Clear();
            for (int i = 0; i < m_RectChildren.Count; i++)
            {
                childrenRow.Add(0);
                childrenX.Add(0);
                childrenY.Add(0);
                childrenSize.Add(Vector2.zero);
            }

            rowsSize.Clear();
            Vector2 currentRowSize = Vector2.zero; 

            for (int i = 0; i < m_RectChildren.Count; i++)
            {
                float w = LayoutUtility.GetPreferredWidth(m_RectChildren[i]); 
                if (currentRowSize.x + w > maxWidth && currentRowSize.x > 0)
                {
                    rowsSize.Add(currentRowSize);
                    currentRowSize = Vector2.zero; 
                }

                childrenX[i] = currentRowSize.x + w / 2; 
                childrenSize[i] = new Vector2(w, 0); 

                currentRowSize.x += w; 

                childrenRow[i] = rowsSize.Count;
                if (i == m_RectChildren.Count-1)
                {
                    rowsSize.Add(currentRowSize); 
                }
            }

            layoutWidth = 0; 
            for (int i = 0; i < rowsSize.Count; i++)
            {
                if(rowsSize[i].x > layoutWidth) layoutWidth = rowsSize[i].x; 
            }

            //Debug.Log("rowsSize: " + rowsSize.GetDump());
            //Debug.Log("childrenRow: " + childrenRow.GetDump());
            //Debug.Log("childrenX: " + childrenX.GetDump());
            //Debug.Log("childrenY: " + childrenY.GetDump());
            //Debug.LogFormat("layout: {0} {1}", layoutWidth, layoutHeight);
        }

        public void CalculateLayoutInputVertical()
        {
            //Debug.Log("CalculateLayoutInputVertical");
            for (int i = 0; i < m_RectChildren.Count; i++)
            {
                float h = LayoutUtility.GetPreferredHeight(m_RectChildren[i]);
                childrenSize[i] = new Vector2(childrenSize[i].x, h);
                if (h > rowsSize[childrenRow[i]].y)
                {
                    rowsSize[childrenRow[i]] = new Vector2(rowsSize[childrenRow[i]].x, h);
                }
            }
            for (int i = 0; i < m_RectChildren.Count; i++)
            {
                childrenY[i] = rowsSize[childrenRow[i]].y / 2;

                for (int r = 0; r < childrenRow[i]; r++)
                {
                    childrenY[i] += rowsSize[childrenRow[r]].y;
                }
            }
            layoutHeight = 0;
            for (int i = 0; i < rowsSize.Count; i++)
            {
                layoutHeight += rowsSize[i].y;
            }
        }
        #endregion 

        #region ILayoutGroup
        Vector3 refPos = Vector3.zero;
        float xOffsetMod = 1;
        float yOffsetMod = 1;
        public void SetLayoutHorizontal()
        { 
            switch (childAlignment)
            {
                case TextAnchor.UpperLeft:
                    refPos = new Vector3(rectTransform.rect.xMin, rectTransform.rect.yMax);
                    xOffsetMod = 1;
                    yOffsetMod = -1;
                    break;
                case TextAnchor.UpperCenter:
                    break;
                case TextAnchor.UpperRight:
                    refPos = new Vector3(rectTransform.rect.xMax, rectTransform.rect.yMax);
                    xOffsetMod = -1;
                    yOffsetMod = -1;
                    break;
                case TextAnchor.MiddleLeft:
                    break;
                case TextAnchor.MiddleCenter:
                    break;
                case TextAnchor.MiddleRight:
                    break;
                case TextAnchor.LowerLeft:
                    refPos = new Vector3(rectTransform.rect.xMin, rectTransform.rect.yMin);
                    xOffsetMod = 1;
                    yOffsetMod = 1;
                    break;
                case TextAnchor.LowerCenter:
                    break;
                case TextAnchor.LowerRight:
                    refPos = new Vector3(rectTransform.rect.xMax, rectTransform.rect.yMin);
                    xOffsetMod = -1;
                    yOffsetMod = 1;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < m_RectChildren.Count; i++)
            {
                m_RectChildren[i].localPosition = refPos + new Vector3(childrenX[i] * xOffsetMod, 0);
                m_RectChildren[i].sizeDelta = new Vector2(childrenSize[i].x, 0);
            }
        }

        public void SetLayoutVertical()
        {
            //Debug.Log("SetLayoutVertical");

            for (int i = 0; i < m_RectChildren.Count; i++)
            {
                m_RectChildren[i].localPosition = refPos + new Vector3(childrenX[i] * xOffsetMod, childrenY[i] * yOffsetMod);
                m_RectChildren[i].sizeDelta = new Vector2(childrenSize[i].x, childrenSize[i].y);
            }
        }
        #endregion
    }
}

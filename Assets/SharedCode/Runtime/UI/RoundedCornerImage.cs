using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class RoundedCornerImage : MaskableGraphic
    {
        [SerializeField]
        private Sprite m_Sprite; 
        public Sprite sprite
        {
            get
            {
                return m_Sprite;
            }
            set
            {
                if (m_Sprite != value)
                {
                    m_Sprite = value;
                    SetAllDirty();
                }
            }
        }

        [SerializeField]
        private float m_Corners = 50;
        public float corners
        {
            get
            {
                return m_Corners;
            }
            set
            {
                if (m_Corners != value)
                {
                    m_Corners = value;
                    SetAllDirty();
                }
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (sprite == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return sprite.texture;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (sprite == null)
            {
                base.OnPopulateMesh(vh);
                return;
            } 

            var color32 = color;
            vh.Clear();

            Rect r = rectTransform.rect; 

            float c = corners;
            if (c < 0) c = 0;
            else
            {
                if (c > r.width / 2) c = r.width / 2;
                if (c > r.height / 2) c = r.height / 2;
            }  

            float[] posX = new float[4];
            posX[0] = r.x;
            posX[1] = r.x + c;
            posX[2] = r.x + r.width - c;
            posX[3] = r.x + r.width;

            float[] posY = new float[4];
            posY[0] = r.y;
            posY[1] = r.y + c;
            posY[2] = r.y + r.height - c;
            posY[3] = r.y + r.height;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    vh.AddVert(
                        new Vector3(posX[i], posY[j]),
                        color32,
                        new Vector2(i / 3f, j / 3f));
                }
            }

            vh.AddTriangle(0, 1, 4); 
            vh.AddTriangle(1, 4, 5);
            vh.AddTriangle(1, 2, 5);
            vh.AddTriangle(2, 5, 6);
            vh.AddTriangle(2, 3, 6);
            vh.AddTriangle(3, 6, 7);

            vh.AddTriangle(4, 5, 8);
            vh.AddTriangle(5, 8, 9);
            vh.AddTriangle(5, 6, 9);
            vh.AddTriangle(6, 9, 10);
            vh.AddTriangle(6, 7, 10);
            vh.AddTriangle(7, 10, 11);

            vh.AddTriangle(8, 9, 12);
            vh.AddTriangle(9, 12, 13);
            vh.AddTriangle(9, 10, 13);
            vh.AddTriangle(10, 13, 14);
            vh.AddTriangle(10, 11, 14);
            vh.AddTriangle(11, 14, 15); 
        }
    }
}
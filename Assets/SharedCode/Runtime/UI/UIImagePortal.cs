using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{ 
    [AddComponentMenu("UI/Effects/Extensions/ImagePortal")] 
    public class UIImagePortal : MaskableGraphic, IUpdateOnMove
    {   
        UpdateOnMove onMoveUpdater;

        RectTransform canvasRect;  
        float cxMin = 0;
        float cyMin = 0;
        float cxMax = 0;
        float cyMax = 0;
        float cw = 0;
        float ch = 0;

        float a = 0;
        float b = 0;
        float c = 0;
        float d = 0;
 
        int i = 1;

        public Sprite imageSprite;
        public override Texture mainTexture{
            get
            {
                return imageSprite == null ? s_WhiteTexture : imageSprite.texture;
            }
        }


        public void OnMove()
        {  
            //a very ugly way to force redraw the mesh, but it works so..
            rectTransform.sizeDelta = rectTransform.sizeDelta + (new Vector2(0,(i=i*-1)))*.01f; 
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {  
            if (canvasRect == null)
            { 
                canvasRect = canvas.GetComponent<RectTransform>();
                cxMin = canvas.transform.position.x + (canvasRect.rect.xMin) * canvas.transform.lossyScale.x;
                cyMin = canvas.transform.position.y + (canvasRect.rect.yMin) * canvas.transform.lossyScale.x;
                cxMax = canvas.transform.position.x + (canvasRect.rect.xMax) * canvas.transform.lossyScale.x;
                cyMax = canvas.transform.position.y + (canvasRect.rect.yMax) * canvas.transform.lossyScale.x;
                cw = canvasRect.rect.width * canvas.transform.lossyScale.x;
                ch = canvasRect.rect.height * canvas.transform.lossyScale.x;
            }  

            if (onMoveUpdater == null)
            {
                onMoveUpdater = GetComponent<UpdateOnMove>();
                if (onMoveUpdater == null)
                {
                    onMoveUpdater = gameObject.AddComponent<UpdateOnMove>();
                }
                onMoveUpdater.updateInEditor = true;
            }

            Vector2 corner1 = Vector2.zero;
            Vector2 corner2 = Vector2.zero;

            corner1.x = 0f;
            corner1.y = 0f;
            corner2.x = 1f;
            corner2.y = 1f;

            corner1.x -= rectTransform.pivot.x;
            corner1.y -= rectTransform.pivot.y;
            corner2.x -= rectTransform.pivot.x;
            corner2.y -= rectTransform.pivot.y;
             

            corner1.x *= rectTransform.rect.width;
            corner1.y *= rectTransform.rect.height;
            corner2.x *= rectTransform.rect.width;
            corner2.y *= rectTransform.rect.height;
 
            a = (rectTransform.position.x + (rectTransform.rect.xMin*rectTransform.lossyScale.x)) - cxMin;
            b = (rectTransform.position.y + (rectTransform.rect.yMin*rectTransform.lossyScale.y)) - cyMin;
            c = (rectTransform.position.x + (rectTransform.rect.xMax*rectTransform.lossyScale.x)) - cxMin;
            d = (rectTransform.position.y + (rectTransform.rect.yMax*rectTransform.lossyScale.y)) - cyMin; 

            vh.Clear();

            UIVertex vert = UIVertex.simpleVert;

            vert.position = new Vector2(corner1.x, corner1.y);
            vert.color = color;
            vert.uv0 = new Vector2(a/cw, b/ch);
            vh.AddVert(vert);

            vert.position = new Vector2(corner1.x, corner2.y);
            vert.color = color;
            vert.uv0 = new Vector2(a/cw, d/ch);
            vh.AddVert(vert);

            vert.position = new Vector2(corner2.x, corner2.y);
            vert.color = color;
            vert.uv0 = new Vector2(c/cw, d/ch);
            vh.AddVert(vert);

            vert.position = new Vector2(corner2.x, corner1.y);
            vert.color = color;
            vert.uv0 = new Vector2(c/cw, b/ch);
            vh.AddVert(vert);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }  
    } 
}
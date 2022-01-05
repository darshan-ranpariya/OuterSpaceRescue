using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMeshInfo : BaseMeshEffect
{
    Image _img;
    Image img
    {
        get
        {
            if (_img == null)
            {
                _img = GetComponent<Image>();
            }
            return _img;
        }
    } 

    RectTransform _rectTransform;
    RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = transform.GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    public float ar
    {
        get
        {
            return (float)img.sprite.texture.width / (float)img.sprite.texture.height;
        }
    } 

    public float w, h;

    List<UIVertex> verts = new List<UIVertex>();
    Vector2 tlp, brp;
    public override void ModifyMesh(VertexHelper vh)
    {
        UIVertex vt = new UIVertex();
        vh.PopulateUIVertex(ref vt, 0); 
        tlp = vt.position;
        vh.PopulateUIVertex(ref vt, 2);
        brp = vt.position;

        w = brp.x - tlp.x;
        h = brp.y - tlp.y;
    }

    public void CalculateLayoutInputHorizontal()
    {
    }

    public void CalculateLayoutInputVertical()
    {

    }
}

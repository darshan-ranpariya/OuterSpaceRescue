using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageElement : LayoutElement
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

    RectTransform _parentRectTransform;
    RectTransform parentRectTransform
    {
        get
        {
            if (_parentRectTransform == null)
            {
                _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            }
            return _parentRectTransform;
        }
    }

    LayoutElement _le;
    LayoutElement le
    {
        get
        {
            if (_le == null)
            {
                _le = GetComponent<LayoutElement>();
            }
            return _le;
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        //Debug.Log("CalculateLayoutInputVertical");
        UpdateEls();
    } 

    float ascpectRatio = 1.75f;

    void UpdateEls()
    { 
        le.preferredWidth = parentRectTransform.rect.width;
        if (img.sprite == null)
        {
            le.preferredHeight = 0;
        }
        else
        {
            ascpectRatio = (float)img.sprite.texture.width / (float)img.sprite.texture.height;
            le.preferredHeight = le.preferredWidth / ascpectRatio;
        }
    } 
}

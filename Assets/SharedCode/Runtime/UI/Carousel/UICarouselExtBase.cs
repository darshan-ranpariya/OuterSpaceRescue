using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UICarousel))]
public abstract class UICarouselExtBase : MonoBehaviour
{
    UICarousel _carousel;
    protected UICarousel carousel
    {
        get
        {
            if (_carousel == null) _carousel = GetComponent<UICarousel>();
            return _carousel;
        }
    }

    public abstract void OnSelectionChanged();
}

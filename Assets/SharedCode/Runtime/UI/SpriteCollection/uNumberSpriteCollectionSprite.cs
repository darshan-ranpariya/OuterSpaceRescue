using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class uNumberSpriteCollectionSprite : uNumberComponent
{
    int i = 0;
    public DynamicSpriteCollection collection;
    public Image img;
    Sprite fbs;

    void Validate()
    {
        if (img == null) img = GetComponent<Image>();
        if (img != null && fbs == null) fbs = img.sprite;
    }

    void OnEnable()
    {
        Validate();
        collection.CollectionUpdated += UpdateImage;
        UpdateImage();
    }

    void OnDisable()
    {
        collection.CollectionUpdated -= UpdateImage;
    }

    private void UpdateImage()
    {
        Validate();
        img.sprite = collection.GetSprite(i);
        if (img.sprite == null) img.sprite = fbs;
    }

    public override void Handle(ref double s)
    {
        i = (int)s;
        UpdateImage();
    }

    void OnValidate()
    {
        Validate();
    }
} 
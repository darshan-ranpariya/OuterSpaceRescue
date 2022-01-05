using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UICarousel))]
public class DynamicOptionsCarousel : MonoBehaviour
{
    [Serializable]
    public class RemapIndex
    {
        public int carouselIndex;
        public int selectionIndex;
    }

    public DynamicOptions selection;
    UICarousel carousel;
    public RemapIndex[] remappedIndexes = new RemapIndex[0];

    void OnEnable()
    {
        if (carousel == null) carousel = GetComponent<UICarousel>();
        carousel.selectionChanged += OnCarouselValChange;
        selection.OptionsUpdated += OnOptionsUpdated;
        selection.LoadingOptions += OnLoadingOptions;
        selection.SelectionChanged += OnSelectionChanged;
        OnOptionsUpdated();
    }

    void OnDisable()
    {
        carousel.selectionChanged -= OnCarouselValChange;
        selection.OptionsUpdated -= OnOptionsUpdated;
        selection.LoadingOptions -= OnLoadingOptions;
        selection.SelectionChanged -= OnSelectionChanged;
    }

    void OnOptionsUpdated()
    {

    }

    void OnSelectionChanged()
    {
        int targetCarouselIndex = selection.selectedIndex;
        for (int i = 0; i < remappedIndexes.Length; i++)
        {
            if (remappedIndexes[i].selectionIndex == selection.selectedIndex)
            {
                targetCarouselIndex = remappedIndexes[i].carouselIndex;
                break;
            }
        }

        if (targetCarouselIndex < carousel.items.Length && carousel.selectedItemIndex != targetCarouselIndex)
        {
            carousel.SelectItem(targetCarouselIndex);
        }
    }

    void OnCarouselValChange()
    {
        int targetSelectionIndex = carousel.selectedItemIndex;
        for (int i = 0; i < remappedIndexes.Length; i++)
        {
            if (remappedIndexes[i].carouselIndex == carousel.selectedItemIndex)
            {
                targetSelectionIndex = remappedIndexes[i].selectionIndex;
                break;
            }
        }

        if (targetSelectionIndex < selection.options.Count && targetSelectionIndex != selection.selectedIndex)
        {
            selection.Select(targetSelectionIndex);
        }
    }

    private void OnLoadingOptions(bool b)
    { 

    }
}

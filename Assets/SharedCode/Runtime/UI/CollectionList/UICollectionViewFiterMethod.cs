using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICollectionViewFiterMethod : MonoBehaviour
{
    public bool removeFilteredItems;
    public UnityEvent OnFilter;

    internal UICollectionViewBase view;
    public uNumber itemsCount;

    public virtual void Filter()
    {
        if (view != null)
        {
            bool f = false;
            int c = 0;
            for (int i = view.items.Count-1; i >= 0; i--)
            {
                f = FilterItem(view.items[i].data, view.items[i].view);
                if (removeFilteredItems && f) view.RemoveItem(view.items[i]);
                else
                {
                    view.items[i].view.gameObject.SetActive(!f);
                }
                if (!f) c++;
            }
            if (view.scroll != null)
            {
                view.scroll.ValidateScroll();
                itemsCount.Value = c;
            }
        }
        OnFilter.Invoke();
        if(view!=null) view.UpdateItemSelection();
    }

    public virtual bool FilterItem(UICollectionItemDataBase data, UICollectionItemViewBase view)
    {
        return false;
    }
}

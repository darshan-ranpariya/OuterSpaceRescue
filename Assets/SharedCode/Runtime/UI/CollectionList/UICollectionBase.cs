using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UICollectionItemDataBase
{
    public int index;
    public virtual bool isRead { get; set; }
    public event Action Updated;
    public void FireUpdated()
    {
        if (Updated != null) Updated();
    }

    public abstract string GetDumpString();
}

public abstract class UICollectionBase : MonoBehaviour/*, ICounterProvider*/
{
    public List<UICollectionItemDataBase> list = new List<UICollectionItemDataBase>();

    public event Action Updated; //when entire list is updated/changed
    public event Action<int, UICollectionItemDataBase> ItemAdded;
    public event Action<int, UICollectionItemDataBase, UICollectionBase> ItemAddedToThis;
    public event Action<int, UICollectionItemDataBase> ItemInserted;
    public event Action<int, UICollectionItemDataBase, UICollectionBase> ItemInsertedToThis;
    public event Action<int, UICollectionItemDataBase> ItemRemoved;
    public event Action<int, UICollectionItemDataBase, UICollectionBase> ItemRemovedFromThis;
    public event Action<int, UICollectionItemDataBase> ItemUpdated;
    public event Action<int, UICollectionItemDataBase, UICollectionBase> ItemUpdatedInThis;
    public event Action Modified;

    public ObservableVariable<int> unreadCount = new ObservableVariable<int>();
    public ObservableVariable<string> status = new ObservableVariable<string>();
    public ObservableVariable<bool> loadingData = new ObservableVariable<bool>();

    public void UpdateList(List<UICollectionItemDataBase> refList)
    {
        list.Clear();
        if (refList!=null)
        {
            for (int i = 0; i < refList.Count; i++)
            {
                refList[i].index = i;
                list.Add(refList[i]);
            }
        }
        if (Updated != null) Updated();
        if (Modified != null) Modified();
        UpdateUnreadCount();
    }

    public void ClearList()
    {
        list.Clear();
        if (Updated != null) Updated();
        if (Modified != null) Modified();
        UpdateUnreadCount();
    }

    public void AddItem(UICollectionItemDataBase item)
    {
        item.index = list.Count;
        list.Add(item);
        if (ItemAdded != null) ItemAdded(list.Count - 1, item); 
        if (ItemAddedToThis != null) ItemAddedToThis(list.Count - 1, item, this);
        if (Modified != null) Modified();
        UpdateUnreadCount();
    }

    public void InsertItem(int i, UICollectionItemDataBase item)
    {
        item.index = i;
        list.Insert(i, item);
        if (ItemInserted != null) ItemInserted(i, item);
        if (ItemInsertedToThis != null) ItemInsertedToThis(i, item, this);

        for (int li = i + 1; li < list.Count; li++)
        {
            list[li].index = li;
            if (ItemUpdated != null) ItemUpdated(li, list[li]);
        }
        if (Modified != null) Modified();

        UpdateUnreadCount();
    }

    [Obsolete("Use the other definition")]
    public void UpdateItem(UICollectionItemDataBase item, int i)
    {
        UpdateItem(i, item);
    }
    public void UpdateItem(int i, UICollectionItemDataBase item)
    {
        if (list[i] != item)
        {
            item.index = i;
            list[i] = item;
        }

        if (ItemUpdated != null) ItemUpdated(i, list[i]);
        if (ItemUpdatedInThis != null) ItemUpdatedInThis(i, list[i], this);
        if (Modified != null) Modified();
        list[i].FireUpdated();
        UpdateUnreadCount();

        //print("ItemUpdated " + i);
    }

    public void UpdateItem<T>(int i, Action<T> act) where T : UICollectionItemDataBase
    {
        act((T)Convert.ChangeType(list[i], typeof(T)));

        if (ItemUpdated != null) ItemUpdated(i, list[i]);
        if (ItemUpdatedInThis != null) ItemUpdatedInThis(i, list[i], this);
        if (Modified != null) Modified();
        list[i].FireUpdated();
        UpdateUnreadCount();
    }

    public void UpdateAllItems<T>(Action<T> act) where T : UICollectionItemDataBase
    {
        for (int i = 0; i < list.Count; i++)
        {
            act((T)Convert.ChangeType(list[i], typeof(T)));
            if (ItemUpdated != null) ItemUpdated(i, list[i]);
            if (ItemUpdatedInThis != null) ItemUpdatedInThis(i, list[i], this);
            list[i].FireUpdated();
        }
        if (Modified != null) Modified();
        UpdateUnreadCount();
    }

    public void IterateAllItems<T>(Action<T> act) where T : UICollectionItemDataBase
    {
        for (int i = 0; i < list.Count; i++)
        {
            act((T)Convert.ChangeType(list[i], typeof(T)));
        }
    }

    public void RemoveItem(UICollectionItemDataBase item)
    {
        int indexBefore = item.index;
        list.Remove(item);
        if (ItemRemoved != null) ItemRemoved(indexBefore, item);
        if (ItemRemovedFromThis != null) ItemRemovedFromThis(indexBefore, item, this);

        for (int li = indexBefore; li < list.Count; li++)
        {
            list[li].index = li;
            if (ItemUpdated != null) ItemUpdated(li, list[li]);
            list[li].FireUpdated();
        }
        if (Modified != null) Modified();

        UpdateUnreadCount();
    }

    public void RemoveItem(int index)
    { 
        UICollectionItemDataBase item = list[index];
        list.RemoveAt(index);
        if (ItemRemoved != null) ItemRemoved(index, item);
        if (ItemRemovedFromThis != null) ItemRemovedFromThis(index, item, this);

        for (int li = index; li < list.Count; li++)
        {
            list[li].index = li;
            if (ItemUpdated != null) ItemUpdated(li, list[li]);
            list[li].FireUpdated();
        }
        if (Modified != null) Modified();

        UpdateUnreadCount();
    }

    public UICollectionItemDataBase GetItem(int i)
    {
        if (i < 0 || i >= list.Count) return null;
        return list[i];
    }

    public T GetItem<T>(int i) where T : UICollectionItemDataBase
    {
        if (i < 0 || i >= list.Count) return null;
        return (T)list[i];
        //return (T)Convert.ChangeType(list[i], typeof(T));
    }

    public int GetItemsCount()
    {
        return list.Count;
    }

    public void MarkItemRead(int index)
    {
        UICollectionItemDataBase item = list[index];
        if (item!=null && !item.isRead)
        {
            item.isRead = true;
            if (ItemUpdated != null) ItemUpdated(index, item);
            if (Modified != null) Modified();
            item.FireUpdated();
            UpdateUnreadCount();
        }
    }

    public void MarkAllRead()
    { 
        for (int i = 0; i < list.Count; i++)
        {
            list[i].isRead = true;
        }
        unreadCount.Value = 0;
    }

    public void UpdateUnreadCount()
    {
        int c = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].isRead) c++;
        }
        unreadCount.Value = c;
    }

    public virtual void LoadItems(object arg)
    {

    }


    //ICounterProvider
    public IObservableVariable<int> count
    {
        get
        {
            return unreadCount;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICollectionItemViewBase : MonoBehaviour
{
    public UICollectionBase m_dataCollection;
    public UICollectionBase dataCollection
    {
        get
        {
            return m_dataCollection;
        }
        set
        {
            if(m_dataCollection != null) m_dataCollection.ItemUpdated -= M_dataCollection_ItemUpdated;
            m_dataCollection = value;
            if (m_dataCollection != null) m_dataCollection.ItemUpdated += M_dataCollection_ItemUpdated;
        }
    }

    public int dataIndex;
    public int dataIndexOnEnable = -1;

    public UICollectionItemDataBase m_data;
    public UICollectionItemDataBase data
    {
        get
        {
            return m_data;
        }
        set
        {
            if (m_data != null) m_data.Updated -= DataUpdated;
            m_data = value;
            if (m_data != null) m_data.Updated += DataUpdated;
            Populate();
        }
    }
    public T GetData<T>()
    {
        if (m_data!=null) return (T)System.Convert.ChangeType(m_data, typeof(T));
        return default(T);
    }
    public GameObject[] onDataObjects;
    public GameObject[] onDataNullObjects;

    public UICollectionViewBase parentView;

    public uBool hasData;
    public uBool isRead;
    public uBool isSelected;
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    private void M_dataCollection_ItemUpdated(int index, UICollectionItemDataBase data)
    {
        if (dataIndexOnEnable >= 0 && index == dataIndexOnEnable)
        {
            this.data = data;
        }
    }

    private void DataUpdated()
    {
        Populate();
    }

    public void OnEnable()
    {
        if (dataIndexOnEnable >= 0)
        {
            if (dataCollection != null && dataCollection.GetItemsCount() > dataIndexOnEnable) data = dataCollection.list[dataIndexOnEnable];
            else data = null;
        }
        Populate();
    }

    protected void Populate()
    {
        hasData.Value = (data != null);
        onDataObjects.SetActive(data != null);
        onDataNullObjects.SetActive(data == null);
        if (data == null) PopulateNullData();
        else
        {
            dataIndex = data.index;
            isRead.Value = data.isRead;
            PopulateData();
        }
    }

    protected virtual void PopulateData()
    {

    }

    protected virtual void PopulateNullData()
    {

    }
    public void Duplicate(UICollectionItemViewBase refItem)
    {
        dataCollection = refItem.dataCollection;
        data = refItem.data; 
    }

    public virtual void MarkRead()
    {
        if (dataCollection != null)
        {
            data.isRead = true;
            dataCollection.UpdateItem(data.index, data);
            dataCollection.UpdateUnreadCount();
        }
        else
        {
            isRead.Value = true;
        }
    } 

    public virtual void Delete()
    {
        if (dataCollection!=null)
        {
            dataCollection.RemoveItem(data);
        }
    }

    public virtual bool IsSelectable()
    {
        return true;
    }

    public virtual void Select()
    { 
        if(parentView!=null) parentView.SelectItem(this); 
    } 

    public virtual void Deselect()
    {
        if (parentView != null) parentView.SelectItem(null);
    }

    public virtual void OnSelect()
    {

    }

    public virtual void OnDeselect()
    {

    }
}

public class UICollectionItemViewBaseLocalized : UICollectionItemViewBase
{
    public void OnEnable()
    {
        base.OnEnable();
        Localization.CurrentLanguageUpdated += Populate;
    }

    public void OnDisable()
    {
        Localization.CurrentLanguageUpdated -= Populate;
    } 
}
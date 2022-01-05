using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionViewBase : MonoBehaviour
{
    [Serializable]
    public class Source
    {
        public UICollectionBase collection;
        public Transform itemPrefab;
    }

    public class Item
    {
        public Source source; 
        public UICollectionItemDataBase data;
        public UICollectionItemViewBase view;
        public bool filterOut;
    }

    public List<Source> sources = new List<Source>();
    public List<Item> items = new List<Item>();
    public ObservableVariable<Item> selectedItem = new ObservableVariable<Item>();

    public Transform itemsParent;
    public Transform refItemsParent;

    public UICollectionViewSortMethod sortMethod;
    public UICollectionViewFiterMethod filterMethod;

    public bool loadItemsOnEnable;
    public bool loadItemsOnServerRoomJoin;

    public bool loadItemsOnPanelOpen;
    public Panel panel;
    

    [UnityEngine.Serialization.FormerlySerializedAs("autoSelectLastItemOnUpdate")]
    public bool autoSelectDefaultItem = true;
    [UnityEngine.Serialization.FormerlySerializedAs("keepSelectionIndexOnUpdate")]
    public bool keepItemSelection;
    public bool invertOrder = true;
    int selectedI = 0;

    public SnapScroll scroll;

    public uString status;
    public uNumber unreadCount;
    public uBool isEmpty;
    public uBool isLoading;


    [Header("Avoid these")]
    public GameObject[] onEmptyObjects = new GameObject[0];
    public GameObject[] onNonEmptyObjects = new GameObject[0];
    public Text[] statusTexts = new Text[0];
    public GameObject[] onLoadingObjects = new GameObject[0];

    public Action ItemSelected;


    void OnEnable()
    {
        refItemsParent.gameObject.SetActive(false); 
        for (int i = 0; i < sources.Count; i++)
        {
            sources[i].itemPrefab.SetParent(refItemsParent, true);
            sources[i].collection.Updated += UpdateView;
            sources[i].collection.ItemAddedToThis += Collection_ItemAddedToThis;
            sources[i].collection.ItemInsertedToThis += Collection_ItemAddedToThis;
            sources[i].collection.ItemRemovedFromThis += Collection_ItemRemovedFromThis;
            sources[i].collection.ItemUpdatedInThis += Collection_ItemUpdatedInThis;
            sources[i].collection.status.Updated += UpdateStatus;
            sources[i].collection.unreadCount.Updated += UpdateUnreadCount;
            sources[i].collection.loadingData.Updated += UpdateLoader;
        } 
         
        //SFS.OnRoomJoined += SFS_OnRoomJoined;
        if(panel!=null) panel.Activated += Panel_Activated;

        if(filterMethod!=null) filterMethod.view = this;

        UpdateView();
        UpdateStatus();
        UpdateUnreadCount();
        UpdateLoader();
        if (loadItemsOnEnable) LoadItems();
        if (keepItemSelection) selectedI = -1;
    }

    private void UpdateUnreadCount()
    {
        int uc = 0;
        for (int i = 0; i < sources.Count; i++)
        {
            uc += sources[i].collection.unreadCount.Value;
        }
        unreadCount.Value = uc;
    }

    void OnDisable()
    {
        for (int i = 0; i < sources.Count; i++)
        {
            sources[i].collection.Updated -= UpdateView;
            sources[i].collection.ItemAddedToThis -= Collection_ItemAddedToThis;
            sources[i].collection.ItemInsertedToThis -= Collection_ItemAddedToThis;
            sources[i].collection.ItemRemovedFromThis -= Collection_ItemRemovedFromThis;
            sources[i].collection.ItemUpdatedInThis -= Collection_ItemUpdatedInThis;
            sources[i].collection.status.Updated -= UpdateStatus;
            sources[i].collection.unreadCount.Updated -= UpdateUnreadCount;
            sources[i].collection.loadingData.Updated -= UpdateLoader;
        }
         
        //SFS.OnRoomJoined -= SFS_OnRoomJoined;
        if (panel != null) panel.Activated -= Panel_Activated;
    }

    //private void SFS_OnRoomJoined(Sfs2X.Entities.Room obj)
    //{
    //    if (loadItemsOnServerRoomJoin) LoadItems();
    //}

    private void Panel_Activated()
    { 
        if (loadItemsOnPanelOpen) LoadItems();
    }

    void LoadItems()
    {
        for (int i = 0; i < sources.Count; i++) sources[i].collection.LoadItems(null);
    }

    public void UpdateView()
    {
        //Debug.Log("PopuplateList", gameObject);
        items.Clear();
        for (int s = 0; s < sources.Count; s++)
        {  
            for (int m = 0; m < sources[s].collection.list.Count; m++)
            {
                items.Add(new Item() {
                    source = sources[s],
                    data = sources[s].collection.list[m]
                });
            }
        }
         
        itemsParent.ClearChildren(); 
        itemsParent.gameObject.SetActive(false);
        if (items.Count > 0)
        {  
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].filterOut) continue;

                items[i].view = Instantiate(items[i].source.itemPrefab, itemsParent, true).GetComponent<UICollectionItemViewBase>();
                items[i].view.dataCollection = items[i].source.collection;
                items[i].view.parentView = this;
                items[i].view.data = items[i].data;
                if(invertOrder) items[i].view.transform.SetAsFirstSibling();
            }
        }

        if(sortMethod!=null) sortMethod.Sort(this);
        if (filterMethod != null) filterMethod.Filter();
        else
        {
            UpdateItemSelection();
            if (scroll != null) scroll.ValidateScroll();
        }

        itemsParent.gameObject.SetActive(true);
         
        
        isEmpty.Value = (items.Count == 0);
        onEmptyObjects.SetActive(items.Count == 0);
        onNonEmptyObjects.SetActive(items.Count > 0);
    } 

    private void Collection_ItemAddedToThis(int index, UICollectionItemDataBase data, UICollectionBase collection)
    {
        Source s = sources.Find((x) => { return x.collection == collection; });
        Item item = new Item();
        item.source = s;
        item.data = data; 
        item.view = Instantiate(s.itemPrefab, itemsParent, true).GetComponent<UICollectionItemViewBase>();
        item.view.dataCollection = collection;
        item.view.parentView = this;
        item.view.data = data;
        items.Add(item);
        if (invertOrder) item.view.transform.SetAsFirstSibling();
         
        if (sortMethod != null) sortMethod.Sort(this);
        if (filterMethod != null) filterMethod.Filter(); 
        else
        {
            UpdateItemSelection();
            if (scroll != null) scroll.ValidateScroll();
        }

        isEmpty.Value = (items.Count == 0);
        onEmptyObjects.SetActive(items.Count == 0);
        onNonEmptyObjects.SetActive(items.Count > 0);
    }

    private void Collection_ItemRemovedFromThis(int index, UICollectionItemDataBase data, UICollectionBase collection)
    {
        Item i = items.Find((x) => { return ((x.source.collection == collection) && (x.data.index == index)); });
        //Item i = items[index];
        RemoveItem(i);

        if (sortMethod != null) sortMethod.Sort(this);
        if (filterMethod != null) filterMethod.Filter(); 
        else
        {
            UpdateItemSelection();
            if (scroll != null) scroll.ValidateScroll();
        }

        isEmpty.Value = (items.Count == 0);
        onEmptyObjects.SetActive(items.Count == 0);
        onNonEmptyObjects.SetActive(items.Count > 0);
    }
    public void RemoveItem(Item i)
    {
        if (i != null)
        {
            items.Remove(i);
            Destroy(i.view.gameObject);
        }
    }

    private void Collection_ItemUpdatedInThis(int index, UICollectionItemDataBase data, UICollectionBase collection)
    {
        Item i = items.Find((x) => { return ((x.source.collection == collection) && (x.data.index == index)); });
        if (i != null)
        {
            i.view.data = data;
        }

        if (sortMethod != null) sortMethod.Sort(this);
        if (filterMethod != null) filterMethod.Filter(); 
        else
        {
            UpdateItemSelection();
            if (scroll != null) scroll.ValidateScroll();
        }

        isEmpty.Value = (items.Count == 0);
        onEmptyObjects.SetActive(items.Count == 0);
        onNonEmptyObjects.SetActive(items.Count > 0);
    }

    void UpdateStatus()
    { 
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < sources.Count; i++)
        {
            if (!string.IsNullOrEmpty(sources[i].collection.status.Value))
            { 
                sb.Append(sources[i].collection.status.Value);
                sb.Append("\n");
                print("UpdateStatus" + sources[i].collection.status.Value);
            }
        }
        statusTexts.SetText(sb.ToString());
        status.Value = sb.ToString();
    }

    void UpdateLoader()
    {
        bool a = false;
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].collection.loadingData.Value)
            {
                a = true;
                break;
            }
        }
        isLoading.Value = a;
        onLoadingObjects.SetActive(a);
    }

    public void UpdateItemSelection()
    {
        //Debug.LogFormat("UpdateItemSelection {0} {1} {2}", name, items.Count, selectedI);

        int i = -1;

        if (keepItemSelection)
        {
            if (selectedI >= 0 && selectedI < items.Count)
            {
                if (items[selectedI].view.gameObject.activeSelf) i = selectedI;
            }
        }

        if (autoSelectDefaultItem)
        {
            if (i == -1)
            {
                for (int ii = 0, iii = 0; ii < items.Count; ii++)
                {
                    iii = invertOrder ? (items.Count - 1 - ii) : ii;

                    if (items[iii].view.gameObject.activeSelf)
                    {
                        if (items[iii].view.IsSelectable())
                        {
                            i = iii;
                            break;
                        }
                    }
                }
            }
        }

        if (keepItemSelection || autoSelectDefaultItem)
        {
            UICollectionItemViewBase siw = null;
            try
            {
                siw = items[i].view;
            }
            catch
            {

            }
            SelectItem(siw);
        }
    }

    public void SelectItem(UICollectionItemViewBase itemView)
    {
        if (itemView !=null && !itemView.IsSelectable()) return;

        //Debug.Log("SelectItem " + itemView.name, itemView.gameObject);
        bool matched = false;
        selectedI = items.FindIndex((x) => { return x.view == itemView; });
        //Debug.Log("SelectItem " + selectedI.ToString().WithColorTag(Color.red));
        for (int i = 0; i < items.Count; i++)
        {
            if (itemView != null && items[i].view == itemView)
            {
                selectedItem.Value = items[i];
                items[i].view.isSelected.Value = true;
                items[i].view.OnSelect();
                items[i].view.OnSelected.Invoke();
                matched = true;
            }
            else
            {
                items[i].view.isSelected.Value = false;
                items[i].view.OnDeselect();
                items[i].view.OnDeselected.Invoke();
            }
        }
        if (!matched) selectedItem.Value = null;
        if (ItemSelected != null) ItemSelected();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (itemsParent == null)
        {
            if (sources.Count > 0)
            {
                if (sources[0].itemPrefab != null)
                {
                    itemsParent = sources[0].itemPrefab.parent;
                    if (refItemsParent == null && itemsParent != null)
                    {
                        refItemsParent = new GameObject("RefItemsParent", typeof(RectTransform)).transform;
                        refItemsParent.SetParent(itemsParent.parent);
                        refItemsParent.localScale = itemsParent.localScale;
                        refItemsParent.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
#endif
}

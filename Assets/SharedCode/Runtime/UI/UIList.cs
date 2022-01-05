using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//[AddComponentMenu("UI/List")]
//[RequireComponent(typeof(ToggleGroup), typeof(ScrollRect))]
using UnityEngine.Events;


public class UIList : MonoBehaviour {

    /*
    How to create one successfullly
    Create GameObject, assign this to that
    Create a child, add scrollrect, image and mask component to that. This is template.
    Create a child of prev child, assign layoutGroup to this, also content size fitter. This is content of above scrollrect
    Create a child of prev child, assign layout element to this. This is you item.
    
    Template will be copied on initialization.
    Item will be copied to the copied template every time new item is created. 
    */

    public Transform template;
    public Transform item; 
    [HideInInspector]
    public List<GameObject> items = new List<GameObject>();
    [HideInInspector]
    public Transform refTemplate; 
    [HideInInspector]
    public Transform itemsParent;
    public GameObject noItemsIndicator;
    public UnityEvent onItemsUpdated;

    public bool supportsSnapping; 
    [HideInInspector]
	public int snappedToItem = 0;
    [Tooltip("number of elements to keep as margin from both sides")]
	public int snapRange = 0;
    Interpolate.Position anim = null;

    public bool retainScrollPos;
    Vector3 lastScrollPos;

    public bool debug;

    void OnEnable()
    { 
        if (debug)
        {
            Debug.Log("OnEnable debug");
        }

        Initialize();

        if (retainScrollPos) itemsParent.localPosition = lastScrollPos;
        else SnapToItem(0, false);

        //InitializeCulling();
    }

    void OnDisable()
    {
        if(itemsParent!=null) lastScrollPos = itemsParent.localPosition;
    }

    void Initialize()
    {
        if (debug)
        {
            Debug.Log(item.parent.name, template.parent.gameObject);
        }

        if (refTemplate == null)
        {
            //refTemplate = template.Duplicate();
            //refTemplate.gameObject.name = "RefTemplate";
            //refTemplate.gameObject.SetActive(false);
            //refTemplate.GetChild(0).ClearChild();
            //item.SetParent(refTemplate.GetChild(0));
            //itemsParent = template.GetChild(0);
            //template.gameObject.SetActive(true);
            //ClearItems(); 
            print(template == null);
            if (debug)
            {
                Debug.Log(item.parent.name, template.parent.gameObject);
            }

            refTemplate = template.Duplicate();
            refTemplate.gameObject.name = "RefTemplate";
            refTemplate.gameObject.SetActive(false);
            refTemplate.ClearChildren();
            itemsParent = item.parent;
            item.SetParent(refTemplate);
            template.gameObject.SetActive(true);
            ClearItems();
        }
    }

    public void ClearItems()
    { 
        Initialize();
        lastScrollPos = itemsParent.localPosition;

        SnapToItem(0,false);
        itemsParent.ClearChildren();
        items.Clear();
        UpdateNoItemsIndicator();
        onItemsUpdated.Invoke();
    }

    public void AddNewItems(int c) 
	{
        for (int i = 0; i < c; i++)
        {
            AddNewItem();
        }
    }

    public GameObject AddNewItem()
    { 
        Initialize();

        if (debug)
        {
            Debug.Log(itemsParent.name.WithColorTag(Color.red), itemsParent.gameObject);
        }
        Transform newItem = item.Duplicate(itemsParent);
        items.Add(newItem.gameObject);
        //SnapToItem (0);
        UpdateNoItemsIndicator();
        onItemsUpdated.Invoke();

        if (retainScrollPos) itemsParent.localPosition = lastScrollPos;
        return newItem.gameObject;  
    }

	public void RemoveItem(int i)
	{
		Destroy (items [i]);
		items.RemoveAt (i);
        UpdateNoItemsIndicator();
        onItemsUpdated.Invoke(); 
	} 

    void UpdateNoItemsIndicator()
    {
        if(noItemsIndicator != null) noItemsIndicator.SetActive(items.Count == 0);
    }

    public void SnapToNextItem() {
        //if (snappedToItem > itemsParent.childCount - snapRange - 1)
        //{
        //    return;
        //}
        SnapToItem(++snappedToItem);
    }

    public void SnapToPrevtItem()
    {
        //if (snappedToItem < snapRange)
        //{
        //    return;
        //}
        SnapToItem(--snappedToItem);
    }

    public void SnapToItem(int i, bool animate = true) {
        if (!supportsSnapping) return;

        if (itemsParent.childCount==0)
        {
            itemsParent.localPosition = Vector3.zero;
            return;
        }
        int lastSanpItem = Mathf.Clamp(itemsParent.childCount - snapRange, 0, itemsParent.childCount-1);
        int firstSnapItem = Mathf.Clamp(snapRange - 1, 0, itemsParent.childCount - 1);
        i = Mathf.Clamp(i, firstSnapItem, lastSanpItem);
        //Debug.LogFormat("i {2}, firstSnapItem {0}, lastSanpItem {1}", firstSnapItem, lastSanpItem, i);
        snappedToItem = i;
        if (anim!=null) anim.Stop();
		if (animate) anim = new Interpolate.Position(itemsParent, itemsParent.localPosition, -itemsParent.GetChild(snappedToItem).localPosition, .3f, true, CurvesCollection.linear);
        else itemsParent.localPosition = -itemsParent.GetChild(snappedToItem).localPosition; 
    } 

 
#region Culling 
    ScrollRect scrollRect; 
    public float cullRange; 

    void InitializeCulling()
    {
        if (cullRange == 0) return;  
        if (scrollRect==null)
        {
            scrollRect = template.GetComponent<ScrollRect>();
            if (scrollRect != null)
            {  
                itemsParent = scrollRect.content.transform;
                scrollRect.onValueChanged.AddListener(ScrollCull);
                ScrollCull(Vector2.zero);
            }
        }
    }

//    int o = 0;
    public void ScrollCull(Vector2 v2)
    { 
//        o = 0;
        for (int i = 0; i < itemsParent.childCount; i++)
        {
            itemsParent.GetChild(i).GetChild(0).gameObject.SetActive(Mathf.Abs(scrollRect.transform.position.x - itemsParent.GetChild(i).position.x) < cullRange);
//            o++;
        }
//        print(o);
    }
#endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapScrollController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler
{
    RectTransform thisRect;
    public SnapScroll scrollview;
    public RectTransform handle;
    public bool horizontal;
    public bool vertical;

    float widthMultiplier { get { return ((scrollview.objectsParentRect.rect.width - scrollview.thisRect.rect.width) / (thisRect.rect.width - handle.rect.width)); } }
    float heightMultiplier { get { return ((scrollview.objectsParentRect.rect.height - scrollview.thisRect.rect.height) / (thisRect.rect.height - handle.rect.height)); } }

    void OnEnable()
    {
        thisRect = GetComponent<RectTransform>();
        scrollview.Scrolled += Scrollview_Scrolled;
        StartCoroutine(DelayedOnEnabe());
    }
    IEnumerator DelayedOnEnabe()
    {
        yield return null;
        UpdateHandle();
    }

    void OnDisable()
    {
        scrollview.Scrolled -= Scrollview_Scrolled;
    }

    void Scrollview_Scrolled()
    {
        if (!pressed)
        {
            UpdateHandle();
        }
    }

    bool pressed = false;
    Vector3 lastPointerPosition;
    Vector2 delta;
    Vector3 handleLocalPos; 
    Vector3 handleLastLocalPos;
    public void OnDrag(PointerEventData eventData)
    {
        OnPointer(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        OnPointer(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointer(eventData);
        pressed = false;
        if (scrollview.autoSnap)
        {
            scrollview.SnapToNearest();
        }
    }

    void OnPointer(PointerEventData eventData)
    {
        Vector3 currPointerPosition = (Camera.main.ScreenToWorldPoint(eventData.position) - thisRect.position);
        if (!horizontal) currPointerPosition.x = 0;
        if (!vertical) currPointerPosition.y = 0;
        currPointerPosition.z = 0;

        handleLocalPos = currPointerPosition / thisRect.lossyScale.x;
        ApplyHandlePos();
        Vector3 offset = scrollview.objectsParentRect.localPosition;
        if (horizontal) offset.x = -handleLocalPos.x * widthMultiplier;
        if (vertical) offset.y = -handleLocalPos.y * heightMultiplier;
        scrollview.SetLocalOffset(offset);
        handleLastLocalPos = handleLocalPos;
        lastPointerPosition = currPointerPosition;
    }

    void UpdateHandle()
    {
        //Debug.LogFormat("{0} {1}", scrollview.objectsParentRect.CalculateHeight(), scrollview.thisRect.CalculateHeight());
        if (scrollview.objectsParentRect.rect.height <= scrollview.thisRect.rect.height)
        {
            handle.gameObject.SetActive(false);
        }
        else
        {
            handle.gameObject.SetActive(true);

            handleLocalPos = -scrollview.objectsParentRect.localPosition;

            if (horizontal) handleLocalPos.x /= widthMultiplier;
            else handleLocalPos.x = 0;

            if (vertical) handleLocalPos.y /= heightMultiplier;
            else handleLocalPos.y = 0;

            ApplyHandlePos();
        }
    }

    void ApplyHandlePos()
    {
        //handleLocalPos.x = Mathf.Clamp(handleLocalPos.x, thisRect.rect.xMin, thisRect.rect.xMax);
        //handleLocalPos.y = Mathf.Clamp(handleLocalPos.y, thisRect.rect.yMin, thisRect.rect.yMax);

        if (handleLocalPos.x + handle.rect.xMax > thisRect.rect.xMax)
        {
            handleLocalPos.x = thisRect.rect.xMax - handle.rect.xMax;
        }
        else if (handleLocalPos.x + handle.rect.xMin < thisRect.rect.xMin)
        {
            handleLocalPos.x = thisRect.rect.xMin - handle.rect.xMin;
        }

        if (handleLocalPos.y + handle.rect.yMax > thisRect.rect.yMax)
        {
            handleLocalPos.y = thisRect.rect.yMax - handle.rect.yMax;
        }
        else if (handleLocalPos.y + handle.rect.yMin < thisRect.rect.yMin)
        {
            handleLocalPos.y = thisRect.rect.yMin - handle.rect.yMin;
        }
        //handleLocalPos.y = Mathf.Clamp(handleLocalPos.y, thisRect.rect.yMin, thisRect.rect.yMax);
        handle.localPosition = handleLocalPos;
    }

    public void OnScroll(PointerEventData eventData)
    {
        Vector2 scrollDelta = Vector2.zero;
        scrollDelta.x = horizontal && !vertical ? (eventData.scrollDelta.x + eventData.scrollDelta.y) : eventData.scrollDelta.x;
        scrollDelta.y = vertical && !horizontal ? (eventData.scrollDelta.x + eventData.scrollDelta.y) : eventData.scrollDelta.y;
        scrollview.WheelScroll(scrollDelta);
    }
}

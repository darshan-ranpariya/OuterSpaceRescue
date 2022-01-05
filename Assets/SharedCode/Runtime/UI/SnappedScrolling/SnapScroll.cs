using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
{
    [HideInInspector]public RectTransform thisRect;
    public RectTransform objectsParentRect;
    public bool horizontal = true;
    public bool vertical = true;
    public bool autoSnap = true;
    float followThroughDuration = 0.5f;
    public float scrollWheelSensitivity = 1;
    public float dragSensitivity = 1;
    public event Action Scrolled; 
    public event Action ScrollStarted;
    public event Action ScrollEnded;
    public event Action SnapStarted; 
    
    internal int currentTargetChild = 0;
    bool onLeftEdge = false;
    bool onRightEdge = false;
    bool onTopEdge = false;
    bool onBottomEdge = false;
    internal Vector3 lastDragDelta = Vector3.zero;
    internal Vector3 lastDragStart = Vector3.zero;
    internal Vector3 lastDragEnd = Vector3.zero;
    internal Vector3 lastDrag = Vector3.zero;
    Coroutine crFollowThrough;
    Coroutine crAutoSnap;
    Coroutine crScrollWheelNormalizer;

    void OnEnable()
    { 
        thisRect = GetComponent<RectTransform>();
        StartCoroutine(DelayedOnEnabe());

        WindowsUtility.SizeChanged += ValidateScrollDelayed;
    }
    IEnumerator DelayedOnEnabe()
    {
        yield return null;
        //SetLocalOffset(Vector3.zero);
        if (autoSnap)
        {
            SnapToNearest();
        }
    }

    void OnDisable()
    {
        WindowsUtility.SizeChanged -= ValidateScrollDelayed;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragStart = eventData.position;
        lastDrag = Vector3.zero;
        if (crFollowThrough!=null) StopCoroutine(crFollowThrough);
        if (crAutoSnap != null) StopCoroutine(crAutoSnap);
        if (ScrollStarted != null) ScrollStarted();
    }

    public void OnDrag(PointerEventData eventData)
    {
        lastDragDelta = new Vector3(eventData.delta.x * thisRect.lossyScale.x, eventData.delta.y * thisRect.lossyScale.y, 0);
        Scroll(lastDragDelta * dragSensitivity);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lastDragEnd = eventData.position;
        lastDrag = lastDragEnd - lastDragStart;
        if (crFollowThrough != null) StopCoroutine(crFollowThrough);
        //crFollowThrough = StartCoroutine(FollowThrough_c());
        if (autoSnap)
        {
            //if (horizontal)
            //{
            //    if (lastDrag.x < 0) SnapNext();
            //    else if (lastDrag.x > 0) SnapPrev();
            //    else SnapToNearest();
            //}
            //else if (vertical)
            //{
            //    if (lastDrag.y > 0) SnapNext();
            //    else if (lastDrag.y < 0) SnapPrev();
            //    else SnapToNearest();
            //}
            //else
                SnapToNearest();
        }
        else
        {
            if (gameObject.activeInHierarchy) crFollowThrough = StartCoroutine(FollowThrough_c());
        }
        if (ScrollEnded != null) ScrollEnded();
    }

    IEnumerator FollowThrough_c()
    {
        Vector3 v3 = Vector3.zero; 
        float t = 0;
        while (t < followThroughDuration)
        {
            yield return null;
            t += Time.deltaTime;
            v3 = Vector3.Lerp(lastDragDelta, Vector3.zero, t/ followThroughDuration);
            Scroll(v3);
        }
        crFollowThrough = null;
        if (autoSnap)
        {
             SnapToNearest();
        }
    }

    public Vector3 Scroll(Vector3 delta)
    {
        if (thisRect == null) return Vector3.zero;

        if (!horizontal) delta.x = 0;
        if (!vertical) delta.y = 0;

        onRightEdge = false;
        float xMaxP = objectsParentRect.position.x + (objectsParentRect.rect.xMax * objectsParentRect.lossyScale.x);
        float xMaxT = thisRect.position.x + (thisRect.rect.xMax * thisRect.lossyScale.x);
        if (xMaxP - xMaxT < -delta.x)
        {
            delta.x = -(xMaxP - xMaxT);
            onRightEdge = true;
        }
        onLeftEdge = false;
        float xMinP = objectsParentRect.position.x + (objectsParentRect.rect.xMin * objectsParentRect.lossyScale.x);
        float xMinT = thisRect.position.x + (thisRect.rect.xMin * thisRect.lossyScale.x);
        if (-(xMinP - xMinT) < delta.x)
        {
            delta.x = -(xMinP - xMinT);
            onLeftEdge = true;
        }

        onBottomEdge = false;
        float yMinT = thisRect.position.y + (thisRect.rect.yMin * thisRect.lossyScale.y);
        float yMinP = objectsParentRect.position.y + (objectsParentRect.rect.yMin * objectsParentRect.lossyScale.y);
        if (-(yMinP - yMinT) < delta.y)
        {
            delta.y = -(yMinP - yMinT);
            onBottomEdge = true;
        }
        onTopEdge = false;
        float yMaxP = objectsParentRect.position.y + (objectsParentRect.rect.yMax * objectsParentRect.lossyScale.y);
        float yMaxT = thisRect.position.y + (thisRect.rect.yMax * thisRect.lossyScale.y);
        if ((yMaxP - yMaxT) < -delta.y)
        {
            delta.y = -(yMaxP - yMaxT);
            onTopEdge = true;
        }

        objectsParentRect.position += delta;
        if(Scrolled!=null) Scrolled();
        return delta;
    }

    public void SetLocalOffset(Vector3 offset)
    {
        if (thisRect == null) return;

        if (!horizontal) offset.x = 0;
        if (!vertical) offset.y = 0;

        onRightEdge = false;
        float xMaxP = (objectsParentRect.rect.xMax);
        float xMaxT = (thisRect.rect.xMax);
        if (xMaxP - xMaxT < -offset.x)
        {
            offset.x = -(xMaxP - xMaxT);
            onRightEdge = true;
        }
        onLeftEdge = false;
        float xMinP = (objectsParentRect.rect.xMin);
        float xMinT = (thisRect.rect.xMin);
        if (-(xMinP - xMinT) < offset.x)
        {
            offset.x = -(xMinP - xMinT);
            onLeftEdge = true;
        }

        onBottomEdge = false;
        float yMinT = thisRect.rect.yMin;
        float yMinP = objectsParentRect.rect.yMin;
        if (-(yMinP - yMinT) < offset.y)
        {
            offset.y = -(yMinP - yMinT);
            onBottomEdge = true;
        }
        onTopEdge = false;
        float yMaxP = objectsParentRect.rect.yMax;
        float yMaxT = thisRect.rect.yMax;
        if ((yMaxP - yMaxT) < -offset.y)
        {
            offset.y = -(yMaxP - yMaxT);
            onTopEdge = true;
        }

        objectsParentRect.localPosition = offset;
        if (Scrolled != null) Scrolled();
    }

    public void ResetScroll()
    {
        //SetLocalOffset(Vector3.zero);
        objectsParentRect.localPosition = Vector3.zero;
        if (Scrolled != null) Scrolled();
    }

    public void ValidateScroll()
    {
        //Scroll(Vector3.zero);
        //Scroll(new Vector3(0.01f, 0.01f, 0));
        if(gameObject.activeInHierarchy) StartCoroutine(ValidateScroll_c());
    }
    IEnumerator ValidateScroll_c()
    {
        yield return null;
        SetLocalOffset(objectsParentRect.anchoredPosition);
        //Scroll(new Vector3(0.01f, 0.01f, 0));
    }

    public void ValidateScrollDelayed()
    {
        new Delayed.Action(ValidateScroll, .1f);
    }

    public void SnapSpecific(int i)
    {
        if (horizontal && vertical) { /*print("ayy");*/ return; }

        //if (i>0)
        //{
        //    if (horizontal && onRightEdge) { /*print("ayy");*/ return; }
        //    if (vertical && onBottomEdge) { /*print("ayy");*/ return; }
        //}
        //if (i < objectsParentRect.childCount)
        //{
        //    if (horizontal && onLeftEdge) { /*print("ayy");*/ return; }
        //    if (vertical && onTopEdge) { /*print("ayy");*/ return; }
        //} 

        currentTargetChild = Mathf.Clamp(i, 0, objectsParentRect.childCount - 1);
        Snap();
    }

    public void SnapNext()
    {
        if (horizontal && vertical) { /*print("ayy");*/ return; }

        if (horizontal && onRightEdge) { /*print("ayy");*/ return; }
        if (vertical && onBottomEdge) { /*print("ayy");*/ return; }

        currentTargetChild = Mathf.Clamp(currentTargetChild + 1, 0, objectsParentRect.childCount - 1);
        Snap();
    }

    public void SnapPrev()
    {
        if (horizontal && vertical) { /*print("ayy");*/ return; }

        if (horizontal && onLeftEdge) { /*print("ayy");*/ return; }
        if (vertical && onTopEdge) { /*print("ayy");*/ return; }

        currentTargetChild = Mathf.Clamp(currentTargetChild - 1, 0, objectsParentRect.childCount - 1);
        Snap();
    }

    public void SnapToNearest()
    {
        if (thisRect == null) return;

        if (currentTargetChild < objectsParentRect.childCount)
        {
            for (int i = 0; i < objectsParentRect.childCount; i++)
            {
                if (Vector3.Distance(objectsParentRect.GetChild(i).position, thisRect.position)
                    < Vector3.Distance(objectsParentRect.GetChild(currentTargetChild).position, thisRect.position))
                {
                    currentTargetChild = i;
                }
            } 
        }
        else currentTargetChild = 0;

        if (currentTargetChild < objectsParentRect.childCount) Snap();
    }

    public void StopSnap()
    {
        if (crAutoSnap != null)
        {
            StopCoroutine(crAutoSnap);
            crAutoSnap = null;
        }
    }
    void Snap()
    {
        if (gameObject.activeInHierarchy)
        {
            StopSnap();
            crAutoSnap = StartCoroutine(SnapToTarget_c());
            if (SnapStarted != null) SnapStarted();
        }
    }
    IEnumerator SnapToTarget_c()
    {
        if (currentTargetChild<0 || currentTargetChild>objectsParentRect.childCount-1)
        {
            yield break;
        } 

        Vector3 travel =  thisRect.position - objectsParentRect.GetChild(currentTargetChild).position;
        Vector3 travelDelta = Vector3.zero;
        Vector3 remainingTravel = travel; 
        
        float t = 0;
        float tt = 0.25f;
        while (t < tt)
        {
            travelDelta = travel * (Time.deltaTime / tt);
            remainingTravel -= Scroll(travelDelta);
            t += Time.deltaTime;
            yield return null;
        }
        Scroll(remainingTravel);
        ValidateScrollDelayed();
        for (int i = 0; i < objectsParentRect.childCount; i++)
        {
            if (Vector3.Distance(objectsParentRect.GetChild(i).position, thisRect.position)
                < Vector3.Distance(objectsParentRect.GetChild(currentTargetChild).position, thisRect.position))
            {
                currentTargetChild = i;
            }
        }
        crAutoSnap = null;
    }
 
    public void OnScroll(PointerEventData eventData)
    {
        WheelScroll(eventData.scrollDelta);
    }
    Vector3 scrollWheelDelta;
    public void WheelScroll(Vector2 scrollDelta)
    {
        scrollWheelDelta.x = horizontal && !vertical ? (scrollDelta.x + scrollDelta.y) : scrollDelta.x;
        scrollWheelDelta.y = vertical && !horizontal ? (scrollDelta.x + scrollDelta.y) : scrollDelta.y;
        scrollWheelDelta = new Vector3(
            -scrollWheelDelta.x * scrollWheelSensitivity * thisRect.lossyScale.x,
            -scrollWheelDelta.y * scrollWheelSensitivity * thisRect.lossyScale.y,
            0);
        Scroll(scrollWheelDelta);
    }
}

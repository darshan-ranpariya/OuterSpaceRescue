using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrollJump2 : MonoBehaviour 
{
    public UITableLayoutGroup tblLayoutGroup;
    public TransformEvents elementsParentEvents;
    public SnapScroll targetScroll;
    public int jumpGap;
    List<Transform> allChildren;
    int[] snapChildren;

    public UISwitch refJumpSwitch;
    bool initSw = false;
    List<UISwitch> switches = new List<UISwitch>();
    public float autoJumpDelay = 5;

    void OnEnable()
    {
        if(tblLayoutGroup!=null) tblLayoutGroup.LayoutUpdated += OnLayoutUpdate;
        if (elementsParentEvents != null) elementsParentEvents.TransformChildrenChanged += OnLayoutUpdate;
        targetScroll.ScrollEnded += OnScrollEnd;
        AutoJump();
    }

    void OnDisable()
    {
        if (tblLayoutGroup != null) tblLayoutGroup.LayoutUpdated -= OnLayoutUpdate;
        if (elementsParentEvents != null) elementsParentEvents.TransformChildrenChanged -= OnLayoutUpdate;
        targetScroll.ScrollEnded -= OnScrollEnd;
    } 

    void OnLayoutUpdate()
    {
        if (tblLayoutGroup != null) jumpGap = tblLayoutGroup.rc * tblLayoutGroup.cc;
        allChildren = targetScroll.objectsParentRect.GetAllActiveChildren();
        snapChildren = new int[Mathf.CeilToInt(allChildren.Count * 1f / jumpGap)];
        for (int i = 0; i < snapChildren.Length; i++)
        {
            snapChildren[i] = allChildren[Mathf.Clamp(i * jumpGap, 0, allChildren.Count-1)].GetSiblingIndex();
        }

        if (initSw == false)
        {
            initSw = true;
            for (int i = refJumpSwitch.transform.parent.childCount - 1; i > 0; i--)
            {
                Destroy(refJumpSwitch.transform.parent.GetChild(i).gameObject);
            }
            switches.Add(refJumpSwitch);
            refJumpSwitch.ThisSwitched += OnSwitchPressed;
        } 
        int totalReq = snapChildren.Length; 
        for (int i = 0; i < refJumpSwitch.transform.parent.childCount; i++)
        {
            refJumpSwitch.transform.parent.GetChild(i).gameObject.SetActive(i < totalReq);
        }
        for (int i = refJumpSwitch.transform.parent.childCount; i < totalReq; i++)
        {
            UISwitch s = refJumpSwitch.transform.Duplicate().GetComponent<UISwitch>();
            switches.Add(s);
            s.ThisSwitched += OnSwitchPressed;
        }
        JumpSpecific(0);
    }

    private void OnSwitchPressed(UISwitch sw, bool b)
    {
        if (b)
        {
            JumpSpecific(sw.transform.GetSiblingIndex());
        } 
    } 

    private void OnScrollEnd()
    {
        tblLayoutGroup.LayoutUpdated -= OnLayoutUpdate;
        targetScroll.StopSnap();
        if (targetScroll.lastDrag.x > 0) JumpPrev();
        else JumpNext();
        tblLayoutGroup.LayoutUpdated += OnLayoutUpdate;
        AutoJump();
    }

    void AutoJump()
    {
        if (autoJumpDelay > 0)
        {
            StopCoroutine("AJ_c");
            StartCoroutine("AJ_c");
        }
    }
    IEnumerator AJ_c()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoJumpDelay);
            bool fb = true;
            for (int i = 0; i < switches.Count; i++)
            {
                if (switches[i].isOn)
                {
                    i = (i + 1) % switches.Count;
                    switches[i].Set(true);
                    fb = false;
                    break;
                }
            }
            if (fb && switches.Count > 0) switches[0].Set(true);
        }
    }

    public void JumpNext()
    {
        if (jumpGap <= 0) return;
        //print("JumpNext " + targetScroll.currentTargetChild);

        for (int i = 0; i < snapChildren.Length; i++)
        {
            if (snapChildren[i] > targetScroll.currentTargetChild)
            {
                JumpSpecific(i);
                return;
            }
        }
        JumpSpecific(snapChildren.Length-1);
    }
    public void JumpPrev()
    {
        if (jumpGap <= 0) return;
        //print("JumpPrev " + targetScroll.currentTargetChild);

        for (int i = snapChildren.Length - 1; i >= 0; i--)
        {
            if (snapChildren[i] < targetScroll.currentTargetChild)
            {
                JumpSpecific(i);
                return;
            }
        }
        JumpSpecific(0);
    }
    public void JumpSpecific(int page)
    {
        //print("JumpSpecific page " + page);
        if(snapChildren.Length > page) targetScroll.SnapSpecific(snapChildren[page]);
        for (int i = 0; i < switches.Count; i++)
        {
            switches[i].ThisSwitched -= OnSwitchPressed;
            switches[i].Set(i == page);
            switches[i].ThisSwitched += OnSwitchPressed;
        }
    } 
} 
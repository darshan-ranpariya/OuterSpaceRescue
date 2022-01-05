using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapScrollJump : MonoBehaviour
{
    public SnapScroll target;
    public UITableLayoutGroup tableLayoutGroup;
    public int startFrom;
    public int jumpGap; 
    public float autoJumpDelay;

    int[] snapPages;

    public UISwitch refJumpSwitch;

    void OnEnable()
    {
        ListenToTarget();
        if (tableLayoutGroup)
        {
            TableLayoutGroup_LayoutUpdated();
        }
        else
        {
            UpdateSwitches();
        }
        AutoJump();
    }

    void OnDisable()
    {
        StopListeningToTarget();
        if (tableLayoutGroup) tableLayoutGroup.LayoutUpdated -= TableLayoutGroup_LayoutUpdated;
    }

    private void TableLayoutGroup_LayoutUpdated()
    {
        jumpGap = (tableLayoutGroup.cc * tableLayoutGroup.rc);
        startFrom = 0;
        UpdateSwitches();
    }

    bool listening = false;
    void ListenToTarget()
    {
        if (listening) return;
        listening = true;
        target.ScrollStarted += Target_ScrollStarted;
        target.ScrollEnded += Target_ScrollEnded; 
        if (tableLayoutGroup) tableLayoutGroup.LayoutUpdated += TableLayoutGroup_LayoutUpdated;
        //elementsParent.TransformChildrenChangedEvent.AddListener(OnElementsChanged);
    }

    void StopListeningToTarget()
    {
        if (!listening) return;
        listening = false;
        target.ScrollStarted -= Target_ScrollStarted;
        target.ScrollEnded -= Target_ScrollEnded; 
        if (tableLayoutGroup) tableLayoutGroup.LayoutUpdated -= TableLayoutGroup_LayoutUpdated;
        //elementsParent.TransformChildrenChangedEvent.RemoveListener(OnElementsChanged);
    }

    void OnElementsChanged()
    {
        UpdateSwitches();
        Target_ScrollEnded();
    }

    int initTarget = 0;
    private void Target_ScrollStarted()
    {
        initTarget = target.currentTargetChild;
    }

    private void Target_ScrollEnded()
    {
        StopListeningToTarget();
        target.StopSnap();

        if (target.lastDrag.x < 0) JumpPrev();
        else JumpNext();

        //if (target.lastDrag.x < 0) JumpToChild(initTarget + jumpGap);
        //else if (target.lastDrag.x > 0)
        //{
        //    if (IsAtTrail()) Jump(((tableLayoutGroup.children.Length / jumpGap) - 1) * jumpGap + startFrom);
        //    JumpToChild(initTarget - jumpGap);
        //}
        //else Jump();

        //if (target.lastDrag.x < 0) JumpNext();
        //else if (target.lastDrag.x > 0) JumpPrev();
        //else Jump();
        ListenToTarget();
        UpdateSwitchesState();
    }

    public void Jump()
    {
        if (jumpGap > 0) Jump((target.currentTargetChild / jumpGap) * jumpGap + startFrom);
    }
    public void JumpNext()
    {
        if (jumpGap > 0)
        {
            // Jump((int)(((target.currentTargetChild * 1f / jumpGap) + 1) * jumpGap + startFrom));
            for (int i = 0; i < snapPages.Length; i++)
            {
                if (snapPages[i] > target.currentTargetChild)
                {
                    target.SnapSpecific(snapPages[i]);
                    break;
                }
            }
        }
    }
    public void JumpPrev()
    {
        if (jumpGap <= 0) return;


        for (int i = snapPages.Length-1; i >= 0; i--)
        {
            if (snapPages[i] < target.currentTargetChild)
            {
                target.SnapSpecific(snapPages[i]);
                break;
            }
        }

        //if (IsAtTrail())
        //{
        //    Jump(((tableLayoutGroup.children.Length / jumpGap) - 1) * jumpGap + startFrom);
        //}
        //else
        //{
        //    Jump(((target.currentTargetChild / jumpGap) - 1) * jumpGap + startFrom);
        //}
    }
    public void JumpNextCycled()
    {
        if (jumpGap > 0)
        {
            if (target.currentTargetChild + jumpGap > tableLayoutGroup.children.Length) Jump(startFrom);
            else Jump(((target.currentTargetChild / jumpGap) + 1) * jumpGap + startFrom);
        }
    }
    public void Jump(int n)
    {
        target.SnapSpecific(n);
        AutoJump();
    }
    public void JumpToChild(int n)
    {
        target.SnapSpecific(((n / jumpGap) * jumpGap) + startFrom);
        AutoJump();
    }

    bool IsAtTrail()
    {
        if (jumpGap <= 0) return false;

        int t = tableLayoutGroup.children.Length - ((tableLayoutGroup.children.Length / jumpGap) * jumpGap);
        if (t > 0)
        {
            t = target.objectsParentRect.childCount - t - startFrom - jumpGap;
        }
        return (t > 0 && target.currentTargetChild >= t);
    }

    bool initSw = false;
    List<UISwitch> switches = new List<UISwitch>();
    void UpdateSwitches()
    {
        if (refJumpSwitch == null) return;
        snapPages = new int[Mathf.CeilToInt(tableLayoutGroup.children.Length * 1f / jumpGap)];
        for (int i = 0; i < snapPages.Length; i++)
        {
            snapPages[i] = startFrom + i * jumpGap;
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

        int totalReq = 0;
        if (jumpGap > 0)
        {
            totalReq = (tableLayoutGroup.children.Length) / jumpGap;
            if (tableLayoutGroup.children.Length % jumpGap > 0) totalReq++;
        }
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
        UpdateSwitchesState();
    }
    void OnSwitchPressed(UISwitch sw, bool b)
    {
        if (b)
        {
            Jump(sw.transform.GetSiblingIndex() * jumpGap + startFrom);
        }
        UpdateSwitchesState();
    }
    void UpdateSwitchesState()
    {
        foreach (var s in switches)
        {
            s.ThisSwitched -= OnSwitchPressed;
            if (jumpGap > 0 && s.transform.GetSiblingIndex() == target.currentTargetChild / jumpGap)
            {
                if (!s.isOn) s.Set(true);
            }
            else
            {
                if (s.isOn) s.Set(false);
            }
            s.ThisSwitched += OnSwitchPressed;
        }
    }

    void AutoJump()
    {
        if (autoJumpDelay>0)
        {
            StopCoroutine("AJ_c");
            StartCoroutine("AJ_c");
        }
    }
    IEnumerator AJ_c()
    {
        yield return new WaitForSeconds(autoJumpDelay);
        for (int i = 0; i < switches.Count; i++)
        {
            if (switches[i].isOn)
            {
                i = (i + 1) % switches.Count;
                switches[i].Set(true);
                break;
            }
        }
        //JumpNextCycled();
        //yield return new WaitForSeconds(1f);
        //UpdateSwitches();
    }
}

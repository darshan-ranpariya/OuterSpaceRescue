using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("UI/Slideout")]
public class UISlideOut : MonoBehaviour {
    public enum OnEnableBehaviour {None, Open, OpenWithAnimation, Close, CloseWithAnimation }

    public Vector3 openPos;
    public Transform openPosRef;
    public Vector3 closePos;
    public Transform closePosRef;
    public float animationTime = .3f;
    [Tooltip("If assigned, it will be set active and inactive on open and close.")]
    public GameObject elements;
    public OnEnableBehaviour onEnable;
    [HideInInspector]
    public bool open;
    Interpolate.Position snAnim; 

    public UnityEvent OnOpen, OnClose;

    void OnEnable() {
        switch (onEnable)
        { 
            case OnEnableBehaviour.Open: 
                Open(false);
                break;

            case OnEnableBehaviour.OpenWithAnimation:
                Close(false);
                Open(true);
                break;

            case OnEnableBehaviour.Close:
                Close(false); 
                break;

            case OnEnableBehaviour.CloseWithAnimation:
                Open(false);
                Close(true); 
                break;

            default:
                break;
        } 
    }

    public void Toggle(bool animate = true)
    {
        if (open)
            Close(animate);
        else
            Open(animate);
    }

    public void Open(bool animate=true)
    {
        UpdateOpenPos();
        open = true;
        if (snAnim != null) snAnim.Stop();
        if (elements!=null)
        {
            elements.SetActive(true);
        }
        if (animate)
        {
            snAnim = new Interpolate.Position(transform, transform.localPosition, openPos, animationTime, true);
        }
        else transform.localPosition = openPos;

        OnOpen.Invoke();
    }
    public void OpenWithAnimation()
    {
        Open(true);
    }

    public void Close(bool animate = true)
    {
        UpdateClosePos();
        open = false;
        if (snAnim != null) snAnim.Stop();
        if (animate)
        {
            snAnim = new Interpolate.Position(transform, transform.localPosition, closePos, animationTime, true);
        }
        else transform.localPosition = closePos;

        if (elements != null)
        {
            Delayed.Function(elements.SetActive, false, animate ? animationTime : 0);
        }
        OnClose.Invoke();
    }
    public void CloseWithAnimation()
    {
        Close(true);
    }

    void UpdateOpenPos() {
        if (openPosRef!=null)
        {
            openPos = openPosRef.localPosition;
        } 
    }

    void UpdateClosePos()
    { 
        if (closePosRef != null)
        {
            closePos = closePosRef.localPosition;
        }
    }
}

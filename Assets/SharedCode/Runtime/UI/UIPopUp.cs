using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class UIPopUp : MonoBehaviour
{
    public enum OnEnableBehaviour
    {
None,
        Open,
        OpenWithAnimation,
        Close,
        CloseWithAnimation

    } 

    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    public float animationTime = .3f;
    public Vector3 closeScale = Vector3.zero, openScale = Vector3.one;
    [Tooltip("If assigned, it will be set active and inactive on open and close.")]
    public GameObject elements;
    public OnEnableBehaviour onEnable;

    [HideInInspector]
    public bool open;
    Interpolate.Scale snAnim;

    public UnityEvent OnOpen, OnClose;

    void OnEnable()
    {
        if (elements == null)
            elements = gameObject;
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

    public void Open(bool animate = true)
    { 
        Debug.LogFormat("Popup open: {0}", gameObject.name);
        open = true;
        if (snAnim != null)
            snAnim.Stop(); 
        elements.SetActive(true); 
        if (animate)
        {
            snAnim = new Interpolate.Scale(elements.transform, closeScale, openScale, animationTime);
        }
        else
            elements.transform.localScale = openScale;

        OnOpen.Invoke();
    }

    public void Close(bool animate = true)
    { 
        open = false;
        if (snAnim != null)
            snAnim.Stop();
        if (animate)
        {
            snAnim = new Interpolate.Scale(elements.transform, openScale, closeScale, animationTime);
        }
        else
            elements.transform.localScale = closeScale; 
 
        Delayed.Function(elements.SetActive, false, animate ? animationTime : 0); 

        OnClose.Invoke();
    } 
}

using System.Collections; 
using UnityEngine;
#if UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif

public class ResolutionLimiter : MonoBehaviour
{  
    public int minWidth = 800;
    public int minHeight = 480;
    public int monitorWidth { get { return Screen.resolutions[Screen.resolutions.Length - 1].width; } }
    public int monitorHeight { get { return Screen.resolutions[Screen.resolutions.Length - 1].height; } }
    public int allowedWidth { get { return monitorWidth < minWidth ? monitorWidth : minWidth; } }
    public int allowedHeight { get { return monitorHeight < minHeight ? monitorHeight : minHeight; } }
    public GameObject indicator;

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);
    public const int VK_LBUTTON = 0x01;
#endif
    
    bool canResize {
        get
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_STANDALONE_WIN
            return (GetAsyncKeyState(VK_LBUTTON) == 0);
#else
            return true;
#endif
        }
    }

    bool animate
    {
        get
        {
            if (!gameObject.activeInHierarchy) return false;
#if UNITY_ANDROID || UNITY_IOS || UNITY_WP8
            return false;
#endif
            return true;
        }
    }

    void OnEnable()
    {

    }

    void Update()
    { 
        if (!animating)
        {
            if (Screen.width < minWidth || Screen.height < minHeight)
            {
                if (indicator != null && !indicator.activeSelf) indicator.SetActive(true);
                if (canResize)
                {
                    SetMinResolution();
                }
            }
            else
            {
                if (indicator != null && indicator.activeSelf) indicator.SetActive(false);
            }
        }
    }


    void SetMinResolution()
    {
        if (indicator != null && indicator.activeSelf) indicator.SetActive(false);
        if (animate)
        {
            if(animating) StopCoroutine("Animation");
            StartCoroutine("Animation");
        }
        else
        { 
            Screen.SetResolution(allowedWidth, allowedHeight, Screen.fullScreen);
        }
    }

    public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
    public float duration = .4f;
    bool animating = false;
    IEnumerator Animation()
    {
        animating = true;
        float t = 0;
        float lf = 0;
        float wm = minWidth * .95f;
        float hm = minHeight * .95f;
        if(wm < Screen.width) wm = Screen.width;
        if(hm < Screen.height) hm = Screen.height;
        int w = allowedWidth;
        int h = allowedHeight; 
        while (t < duration)
        {
            lf = curve.Evaluate(t / duration);
            Screen.SetResolution((int)(Mathf.Lerp(wm, w, lf)), (int)(Mathf.Lerp(hm, h, lf)), Screen.fullScreen);
            t += Time.deltaTime;
            yield return null;
        }
        Screen.SetResolution(w, h, Screen.fullScreen); 
        animating = false;
    }
     
}

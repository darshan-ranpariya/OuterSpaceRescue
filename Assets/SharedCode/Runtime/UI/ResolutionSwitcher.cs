using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSwitcher : MonoBehaviour
{
    [Serializable]
    public class Resolution
    {
        public int width;
        public int height;
        public Toggle toggle;
    }

    public Resolution[] resolutions;
    public Toggle setPreferredToggle;
    public int defWidth;
    public int defHeight;
    public Panel panel;

    public int prefWidth
    {
        get { return PlayerPrefs.GetInt("resW"); }
        set { PlayerPrefs.SetInt("resW", value); }
    }

    public int prefHeight
    {
        get { return PlayerPrefs.GetInt("resH"); }
        set { PlayerPrefs.SetInt("resH", value); }
    }

    void OnEnable()
    {
        int aw = prefWidth;
        int ah = prefHeight;
        if (aw == 0 || ah == 0)
        {
            aw = defWidth;
            ah = defHeight;
        }
#if !UNITY_EDITOR
        Screen.SetResolution(aw, ah, Screen.fullScreen);
#else
        Debug.LogFormat("Set Res: {0} x {1}", aw, ah);
#endif
        panel.Activated += Panel_Activated;
    }

    void OnDisable()
    {
        panel.Activated -= Panel_Activated;
    }

    private void Panel_Activated()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutions[i].toggle.isOn = (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height);
        }
        setPreferredToggle.isOn = true;
    }

    public void ApplyResolution()
    {
        int aw = Screen.width;
        int ah = Screen.height;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].toggle.isOn)
            {
                if (setPreferredToggle.isOn)
                {
                    prefWidth = resolutions[i].width;
                    prefHeight = resolutions[i].height;
                }
                if (aw != resolutions[i].width || ah != resolutions[i].height)
                {
                    aw = resolutions[i].width;
                    ah = resolutions[i].height;
#if !UNITY_EDITOR
                    Screen.SetResolution(aw, ah, Screen.fullScreen);
#else
                    Debug.LogFormat("Set Res: {0} x {1}", aw, ah);
#endif
                }
                break;
            }
        }
    }
}

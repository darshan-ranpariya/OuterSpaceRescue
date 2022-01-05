using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionUtils : MonoBehaviour
{
    public int minWidth, minHeight;
    public GameObject lowResolutionIndicator;

    void Update()
    {
        if (lowResolutionIndicator!=null)
        {
            if (minHeight > 0 && minWidth > 0)
            {
                if (lowResolutionIndicator.activeSelf)
                {
                    if (Screen.width >= minWidth && Screen.height >= minHeight)
                    {
                        lowResolutionIndicator.SetActive(false);
                    }
                }
                else
                {
                    if (Screen.width < minWidth || Screen.height < minHeight)
                    {
                        lowResolutionIndicator.SetActive(true);
                    }
                } 
            }
        }
    }

    public void SetMinimumResolution()
    {
        if (minHeight > 0 && minWidth > 0)
        {
            Screen.SetResolution(minWidth, minHeight, Screen.fullScreen);
        }
    }
}

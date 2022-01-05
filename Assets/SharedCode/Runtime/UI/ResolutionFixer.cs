using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionFixer : MonoBehaviour
{
    [Serializable]
    public class ResolutionPreset
    {
        public int width = 0;
        public int height = 0;
    }
    public ResolutionPreset defaultResolution;

    void OnEnable()
    {
        Screen.SetResolution(defaultResolution.width, defaultResolution.height, Screen.fullScreen);
    }   
     
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetFontFilteringToPoint : MonoBehaviour
{
    public FilterMode filterMode = FilterMode.Point;
    void Start()
    {
        Text t = GetComponent<Text>();
        if(t != null) t.font.material.mainTexture.filterMode = filterMode;
    }
}

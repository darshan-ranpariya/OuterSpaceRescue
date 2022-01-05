#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEditor;

public class TMProPort : EditorWindow
{
    public TMP_FontAsset fontAsset;
    public GameObject refObj;

    [MenuItem("Window/TextMeshPro/Port")] 
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TMProPort)); 
    }

    void OnGUI()
    {
        fontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField(fontAsset, typeof(TMP_FontAsset), true);
        refObj = (GameObject)EditorGUILayout.ObjectField(refObj, typeof(GameObject), true);
        if (GUILayout.Button("Port"))
        {
            Port();
        }
    }

     
    Vector2 sizeDelta;

    string txt = "";
    int size;
    bool autoSize;
    int minSize;
    int maxSize; 
    TextAnchor al;
    Color col = Color.white;

    bool hasGrad = false; 
    Color grCol = Color.white;
    Color grCol2 = Color.white;
    
    RectTransform rt = null;
    Text textComp = null;
    //UnityEngine.UI.Extensions.Gradient gr = null;
    Shadow sh = null;
    SetFontFilteringToPoint ff = null;
    //LetterSpacing ls = null;
    TextMeshProUGUI tmpComp;

    void Port()
    {
        if (refObj == null) return;

        if (!refObj.activeInHierarchy)
        {
            Debug.LogError("gameobject is inactive.");
            return;
        }

        rt = (RectTransform)(refObj.transform);
        textComp = refObj.GetComponent<Text>();
        //gr = refObj.GetComponent<UnityEngine.UI.Extensions.Gradient>();
        sh = refObj.GetComponent<Shadow>();
        ff = refObj.GetComponent<SetFontFilteringToPoint>();
        //ls = refObj.GetComponent<LetterSpacing>();

        sizeDelta = rt.sizeDelta;

        if (textComp != null)
        {
            size = textComp.fontSize;
            autoSize = textComp.resizeTextForBestFit;
            minSize = textComp.resizeTextMinSize;
            maxSize = textComp.resizeTextMaxSize;
            al = textComp.alignment;
            txt = textComp.text;
            col = textComp.color;
        }
        else
        {
            txt = "";
            size = 14;
            autoSize = false;
            minSize = 5;
            maxSize = 14;
            al = TextAnchor.UpperLeft;
            col = Color.white;
        }

        //hasGrad = (gr != null);
        //if (hasGrad)
        //{
        //    grCol = gr.vertex1;
        //    grCol2 = gr.vertex2;
        //}

        if(textComp) DestroyImmediate(textComp);
        //if (gr) DestroyImmediate(gr);
        if (sh) DestroyImmediate(sh);
        if (ff) DestroyImmediate(ff);
        //if (ls) DestroyImmediate(ls);

        tmpComp = refObj.AddComponent<TextMeshProUGUI>();
        tmpComp.fontSize = size;
        tmpComp.font = fontAsset;
        tmpComp.color = col;

        switch (al)
        {
            case TextAnchor.UpperLeft:
                tmpComp.alignment = TextAlignmentOptions.TopLeft;
                break;
            case TextAnchor.UpperCenter:
                tmpComp.alignment = TextAlignmentOptions.Top;
                break;
            case TextAnchor.UpperRight:
                tmpComp.alignment = TextAlignmentOptions.TopRight;
                break;
            case TextAnchor.MiddleLeft:
                tmpComp.alignment = TextAlignmentOptions.MidlineLeft;
                break;
            case TextAnchor.MiddleCenter:
                tmpComp.alignment = TextAlignmentOptions.Midline;
                break;
            case TextAnchor.MiddleRight:
                tmpComp.alignment = TextAlignmentOptions.MidlineRight;
                break;
            case TextAnchor.LowerLeft:
                tmpComp.alignment = TextAlignmentOptions.BottomLeft;
                break;
            case TextAnchor.LowerCenter:
                tmpComp.alignment = TextAlignmentOptions.Bottom;
                break;
            case TextAnchor.LowerRight:
                tmpComp.alignment = TextAlignmentOptions.BottomRight;
                break;
            default:
                break;
        }
        if (autoSize)
        {
            tmpComp.enableAutoSizing = autoSize;
            tmpComp.fontSizeMin = minSize;
            tmpComp.fontSizeMax = maxSize;
        }
        if (hasGrad)
        {
            tmpComp.enableVertexGradient = true;
            tmpComp.colorGradient = new VertexGradient(grCol, grCol, grCol2, grCol2);
        }

        tmpComp.text = txt;

        ((RectTransform)(refObj.transform)).sizeDelta = sizeDelta;
        //refObj.SetActive(objEnabled);
    }
}

#endif
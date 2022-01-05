using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LinkedTexts : MonoBehaviour {
    public Text thisText;
    public TMP_Text thisTextPro;
    public Text[] targetTexts = new Text[0];
    public TMP_Text[] targetTextsPro = new TMP_Text[0];


    [Header("Linked Properties")]
    public bool alignment;
    public bool size;

    [Header("Player")]
    public bool p_OnEnable;
    public bool p_OnUpdate;

    [Header("Editor")]
    public bool e_OnEnable;
    public bool e_OnUpdate;

    void OnEnable()
    {
        thisText = GetComponent<Text>();
        if (Application.isPlaying) { if (!p_OnEnable) return; }
        else { if (!e_OnEnable) return; }
        Link();
    }

    void Update()
    {
        if (Application.isPlaying) { if (!p_OnUpdate) return; }
        else { if (!e_OnUpdate) return; }
        Link();
    }

    public void Link()
    {
        for (int i = 0; i < targetTexts.Length; i++)
        {
            if (!targetTexts[i]) continue;
            if (alignment) targetTexts[i].alignment = thisText.alignment;
            if (size) targetTexts[i].fontSize = thisText.fontSize; 
        }
        for (int i = 0; i < targetTextsPro.Length; i++)
        {
            if (!targetTextsPro[i]) continue;
            if (alignment) targetTextsPro[i].alignment = thisTextPro.alignment;
            if (size) targetTextsPro[i].fontSize = thisTextPro.fontSize;
        }
    }
}

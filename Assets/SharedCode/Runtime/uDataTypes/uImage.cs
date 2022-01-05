using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class uImage : uVar<Sprite, uImageComponent>
{ 
    public Image[] imgComps = new Image[] { };
     
    public Sprite fallbackSprite; 

    public string _url;
    public string url
    {
        get
        {
            return _url;
        }
        set
        {
            if (_url != null && value != null && _url.Equals(value)) return;

            _url = value;
            Value = Download.Image.transparentSprite;

            if (_url == null) return;

            if (imgComps.Length > 0 && _url != null && !string.IsNullOrEmpty(_url))
            {
                new Download.Image(value, OnDownload, true, null, fallbackSprite);
            }
        }
    }

    void OnDownload(Sprite _sprite)
    {
        Value = _sprite;
    }

    public override void OnValueChanged()
    {
        if (imgComps != null)
        {
            for (int i = 0; i < imgComps.Length; i++)
            {
                if (imgComps[i] != null)
                {
                    if (m_value == null)
                    {
                        if (fallbackSprite != null) imgComps[i].sprite = fallbackSprite; 
                        else imgComps[i].sprite = Download.Image.transparentSprite;
                    }
                    else imgComps[i].sprite = m_value;
                }
            }
        }
    }
}


public abstract class uImageComponent : MonoBehaviour, IuVarHandler<Sprite>
{
    public abstract void Handle(ref Sprite s);
} 


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(uImage))]
public class uImageDrawer : uVarDrawer
{
    string[] arr = new string[] { "imgComps", "components" };
    public override string[] arrays
    {
        get
        {
            return arr;
        }
    }
    protected override int GetUnFoldedContentLines()
    {
        return 1;
    }
    protected override int GetUnFoldedContentSpacings()
    {
        return 0;
    }

    public override void DrawFoldoutContent(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect r = GetNextRect(); 
        GUI.Label(GetLabelArea(r), "Fallback");
        EditorGUI.PropertyField(GetValueArea(r), property.FindPropertyRelative("fallbackSprite"), GUIContent.none);
        //EditorGUI.PropertyField(GetNextRect(), property.FindPropertyRelative("fallbackSprite"), new GUIContent("Fallback"));
    }
}
#endif
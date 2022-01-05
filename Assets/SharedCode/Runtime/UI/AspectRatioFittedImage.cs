using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Image), typeof(AspectRatioFitter))]
public class AspectRatioFittedImage : MonoBehaviour
{
    AspectRatioFitter _arf;
    AspectRatioFitter arf
    {
        get
        {
            if(_arf == null) _arf = GetComponent<AspectRatioFitter>();
            return _arf;
        }
    }

    Image _img;
    Image img
    {
        get
        {
            if (_img == null) _img = GetComponent<Image>();
            return _img;
        }
    } 

    void OnEnable()
    { 
        UpdateAspectRatio();
    }

    public void SetSprite(Sprite s)
    {
        img.sprite = s;
        UpdateAspectRatio();
    }

    public void UpdateAspectRatio()
    {
        float r = 1;
        if (img.sprite != null)
        {
            r = img.sprite.texture.width / img.sprite.texture.height;
        }
        arf.aspectRatio = r;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AspectRatioFittedImage))]
[CanEditMultipleObjects]
public class AspectRatioFittedImageEditor : Editor
{
    AspectRatioFittedImage script;
    void OnEnable()
    {
        script = target as AspectRatioFittedImage;
        script.UpdateAspectRatio();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update"))
        {
            script.UpdateAspectRatio();
        }
    }
}
#endif
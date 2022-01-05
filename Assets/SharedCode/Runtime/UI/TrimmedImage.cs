using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrimmedImage : Image
{
    public enum TrimSide
    {
        Width,
        Height
    }

    public TrimSide m_trim;

    float ar
    {
        get
        {
            if (sprite == null) return 0;
            return (float)sprite.texture.width / (float)sprite.texture.height;
        }
    }

    float w = 0;
    float h = 0;
    public override float preferredWidth
    {
        get
        {
            w = base.preferredWidth; 
            if (m_trim == TrimSide.Width && ar != 0 && w > rectTransform.rect.height * ar)
            {
                w = rectTransform.rect.height * ar;
            }
            return w;
        }
    }

    public override float preferredHeight
    {
        get
        {
            h = base.preferredHeight; 
            if (m_trim == TrimSide.Height && ar != 0 && h > rectTransform.rect.width / ar)
            {
                h = rectTransform.rect.width / ar;
            }
            return h;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        base.OnValidate();

        base.type = Type.Simple;
        base.preserveAspect = true;
    }
#endif

}


#if UNITY_EDITOR
[CustomEditor(typeof(TrimmedImage))]
public class TrimmedImageEditor : Editor
{
    TrimmedImage renderer;
    void OnEnable()
    {
        renderer = target as TrimmedImage;
    }

    public override void OnInspectorGUI()
    { 
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Sprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Color"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Material")); 
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_trim"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

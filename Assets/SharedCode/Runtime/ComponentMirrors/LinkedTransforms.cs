using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class LinkedTransforms : MonoBehaviour
{
    public RectTransform thisRect;
    public RectTransform[] targetRects = new RectTransform[0];

    [Header("Linked Properties")]
    public bool position = true;
    public bool rotation = false;
    public bool scale = false;
    public bool size = false;
    public bool pivot = false;
    public bool anchors = false;

    [Header("Player")]
    public bool p_OnEnable;
    public bool p_OnUpdate;

    [Header("Editor")]
    public bool e_OnEnable;    
    public bool e_OnUpdate;

    void OnEnable()
    {
        thisRect = GetComponent<RectTransform>();
        if (Application.isPlaying) { if (!p_OnEnable) return; }
        else { if (!e_OnEnable) return; }
        Link();
        StartCoroutine(DelayedEnable());
    }
    IEnumerator DelayedEnable()
    {
        yield return null;
        Link(); 
    }

    void Update ()
    {
        if (Application.isPlaying) { if (!p_OnUpdate) return; }
        else { if (!e_OnUpdate) return; }

        //if (!Application.isPlaying) { if (!e_OnUpdate) return; }

        Link();
    }

    public void Link()
    {
        for (int i = 0; i < targetRects.Length; i++)
        {
            if (position) targetRects[i].position = thisRect.position;
            //if (position) targetRects[i].position = Vector3.Lerp(targetRects[i].position, thisRect.position, 0.1f);
            if (rotation) targetRects[i].rotation = thisRect.rotation;
            if (scale) targetRects[i].localScale = thisRect.localScale;
            if (size) targetRects[i].sizeDelta = thisRect.sizeDelta;
            //if (size) targetRects[i].sizeDelta = Vector2.Lerp(targetRects[i].sizeDelta, thisRect.sizeDelta, 0.1f);
            if (pivot) targetRects[i].pivot = thisRect.pivot;
            if (anchors)
            {
                targetRects[i].anchorMin = thisRect.anchorMin;
                targetRects[i].anchorMax = thisRect.anchorMax;
            }
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(LinkedTransforms))]
[CanEditMultipleObjects]
public class LinkedTransformsEditor : Editor
{
    LinkedTransforms script;
    void OnEnable()
    {
        script = target as LinkedTransforms; 
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 
        if (GUILayout.Button("Link")) script.Link(); 
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode] 
public abstract class LinkedComponentsBase<T> : MonoBehaviour
{ 
    public T refComp;
    public T[] targetComps = new T[0]; 

    [Header("Player")]
    public bool p_OnEnable;
    public bool p_OnUpdate;

    [Header("Editor")]
    public bool e_OnEnable;
    public bool e_OnUpdate;

    void OnEnable()
    {
        if (refComp == null) refComp = GetComponent<T>();

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
        for (int i = 0; i < targetComps.Length; i++)
        {
            LinkComp(refComp, targetComps[i]);
        }
    }

    public abstract void LinkComp(T refComp, T targetComp);
}


#if UNITY_EDITOR 
[CanEditMultipleObjects]
public class LinkedComponentsEditorBase : Editor
{
    Transform script;
    void OnEnable()
    {
        script = target as Transform;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Link")) script.SendMessage("Link");
    }
}
#endif 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

//[ExecuteInEditMode]
public class uAnimatorEditorPlayer : uAnimatorExt
{ 
    [Range(0, 1)]
    public float value = 0; 
    [HideInInspector]
    public float lastValue = 0;

    //void Update()
    //{
    //    if (lastValue != value)
    //    {
    //        anim.value = value;
    //    }
    //    lastValue = value;
    //}
}

#if UNITY_EDITOR
[CustomEditor(typeof(uAnimatorEditorPlayer))]
public class uAnimatorEditorPlayerEditor : Editor
{
    uAnimatorEditorPlayer script;

    void OnEnable()
    {
        script = target as uAnimatorEditorPlayer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.changed)
        {
            if (script.lastValue != script.value)
            {
                script.anim.value = script.value;
            }
            script.lastValue = script.value;
        }
    }
}
#endif

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorNote : MonoBehaviour {
    public string note="Please note:\n";
}

#if UNITY_EDITOR
[CustomEditor(typeof(EditorNote))]
public class EditorNoteEditor : Editor 
{
    EditorNote script;
    void OnEnable()
    {
        script = target as EditorNote;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        script.note = GUILayout.TextArea(script.note);
    }
}
#endif
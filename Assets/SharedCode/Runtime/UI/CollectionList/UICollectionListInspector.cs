#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UICollectionListInspector : MonoBehaviour
{
    public UICollectionBase m_collection;
    public UICollectionBase collection
    {
        get
        {
            if (m_collection == null) m_collection = GetComponent<UICollectionBase>();
            return m_collection;
        }
    }
}

[CustomEditor(typeof(UICollectionListInspector))]
public class UICollectionListInspectorEditor : Editor
{
    public UICollectionListInspector script;
    void OnEnable()
    {
        script = target as UICollectionListInspector;
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < script.collection.list.Count; i++)
        {
            GUILayout.Label(script.collection.list[i].GetDumpString());
        }
    }
}
#endif

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerPrefsInspector : MonoBehaviour {
#if UNITY_EDITOR
    [System.Serializable]
    public class StringPref {
        public string key;
        public string val;
    }
    [System.Serializable]
    public class IntPref
    {
        public string key;
        public int val;
    }
    [System.Serializable]
    public class FloatPref
    {
        public string key;
        public float val;
    }

    public StringPref[] stringPrefs;
    public IntPref[] intPrefs;
    public FloatPref[] floatPrefs;

    public void UpdateVals () {
        for (int i = 0; i < stringPrefs.Length; i++)
        {
            stringPrefs[i].val = PlayerPrefs.GetString(stringPrefs[i].key);
        }
        for (int i = 0; i < intPrefs.Length; i++)
        {
            intPrefs[i].val = PlayerPrefs.GetInt(intPrefs[i].key);
        }
        for (int i = 0; i < floatPrefs.Length; i++)
        {
            floatPrefs[i].val = PlayerPrefs.GetFloat(floatPrefs[i].key);
        }
    } 
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(PlayerPrefsInspector))]
public class PlayerPrefsPInspectorEditor : Editor
{
    PlayerPrefsInspector script;
    void OnEnable() {
        script = target as PlayerPrefsInspector;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update"))
        {
            script.UpdateVals();
        }
    }
}
#endif
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIToggleBgColor : MonoBehaviour 
{
    public Toggle toggle; 
    public Color onColor = Color.white, offColor = Color.white;
    void OnEnable()
    {
        toggle.onValueChanged.AddListener(OnToggleValChange);
        OnToggleValChange(toggle.isOn);
    }
    void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValChange);
    }
    void OnToggleValChange(bool b)
    {
        UpdateColor();    
    }
    public void UpdateColor()
    { 
        toggle.image.color = toggle.isOn ? onColor : offColor;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIToggleBgColor))]
public class UIToggleBgColorEditor : Editor
{
    UIToggleBgColor script;
    void OnEnable()
    {
        script = target as UIToggleBgColor;
        script.UpdateColor();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.changed)
        {
            script.UpdateColor();
        }
    }
}
#endif
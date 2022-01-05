using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIToggleLabelColor : MonoBehaviour 
{ 
    public Toggle toggle;
    public Text label;
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
        label.color = toggle.isOn ? onColor : offColor;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIToggleLabelColor))]
public class UIToggleLabelColorEditor : Editor
{
    UIToggleLabelColor script;
    void OnEnable()
    {
        script = target as UIToggleLabelColor;
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
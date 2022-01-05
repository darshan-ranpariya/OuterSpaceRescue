using UnityEngine;
using UnityEngine.EventSystems;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UISwitch : uBoolComponent, IPointerClickHandler
{
    public enum OnClickAction {  Toggle, SetOn, SetOff, DoNothing }

    public UISwitchGroup group;
    public OnClickAction onClick;
    public event Action SwitchedOn, SwitchedOff;
    public event Action<bool> Switched;
    public event Action<UISwitch, bool> ThisSwitched;

    UISwitchExtension[] extensions;
    public bool isOn { get { return _isOn; } }
    [HideInInspector]
    public bool _isOn;

    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        UpdateSwitch();
    }

    void OnDestroy()
    { 
        if (group != null) group.RemoveSwitch(this);
    }

    bool init = false;
    public void Init()
    {
        if (init) return;
        init = true;
        //extensions = GetComponents<UISwitchExtension>();
        if (group != null) group.AddSwitch(this);
        extensions = GetComponents<UISwitchExtension>();
        if (extensions != null)
        {
            for (int i = 0; i < extensions.Length; i++) extensions[i].Init(this);
        }
        //Set(_isOn);
        UpdateSwitch();
    }

    public void Toggle()
    { 
        Set(!_isOn);
    }

    public void Set(bool on)
    {
        //if (_isOn == on) return;

        if (group != null && on) group.OnSwitchOn(this);

        _isOn = on;

        UpdateSwitch();

        if (isOn && SwitchedOn!=null) SwitchedOn();
        else if (SwitchedOff != null) SwitchedOff();
        if (Switched != null) Switched(_isOn);
        if (ThisSwitched != null) ThisSwitched(this, _isOn);
    }

    public void UpdateSwitch()
    {
        extensions = GetComponents<UISwitchExtension>();
        if (extensions != null)
        {
            for (int i = 0; i < extensions.Length; i++) extensions[i].OnSwitchValChanged(_isOn);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    { 
        OnClick();
    }

    public virtual void OnClick()
    {
        switch (onClick)
        {
            case OnClickAction.Toggle:
                Toggle();
                break;
            case OnClickAction.SetOn:
                Set(true);
                break;
            case OnClickAction.SetOff:
                Set(false);
                break;
            default:
                break;
        }
    }

    public override void Handle(ref bool s)
    {
        Set(s);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(UISwitch))]
[CanEditMultipleObjects]
public class UISwitchEditor : Editor
{
    UISwitch script;
    void OnEnable()
    {
        script = target as UISwitch;
        script.Init();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        EditorGUILayout.Toggle(script.isOn);
        if(GUILayout.Button("Set On")) script.Set(true);
        if(GUILayout.Button("Set Off")) script.Set(false);
        GUILayout.EndHorizontal();
    }
}
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Panel : MonoBehaviour 
{ 
    bool _isActive; 
    public bool isActive { get { Init(); return _isActive; } set { Init();  _isActive = value; } }
    public PanelGroup group;
    public AnimationBase openAnim, closeAnim; 

    public List<System.Func<bool>> validations = new List<System.Func<bool>>();
    public event System.Action Activated,Deactivated;

    void Awake()
    {
        Init();
    }

    void OnDisable()
    {
        if (openAnim) openAnim.StopAnim();
        if (closeAnim) closeAnim.StopAnim();
        if (!isActive)
        {
            DeactivateGameObject();
        }
    }

    bool init;
    void Init()
    {
        if (!init)
        {
            if (group != null)
            {
                group.RegisterPanel(this);
            }

            _isActive = gameObject.activeSelf;

            if (closeAnim != null)
            {
                closeAnim.AddCallbackOnEnd(DeactivateGameObject);
            }
            init = true;
        }
    }
    public void Toggle()
    {
        Toggle(!_isActive);
    } 

    public void Toggle(bool _activate)
    {
        if (_activate) Activate();
        else Deactivate();
    }

    public void Activate()
    {
        Init();

        for (int i = 0; i < validations.Count; i++)
        {
            try { if (!validations[i]()) return; }
            catch {}
        }

        if (_isActive) return;
        _isActive = true;

        //Debug.Log("Panel Activated: " + name);

        ActivateGameObject();
        if (Application.isPlaying && gameObject.activeInHierarchy && openAnim != null)
        {
            if (closeAnim != null)
                closeAnim.StopAnim();
            
            openAnim.StartAnim();
        }

        if (group != null) group.OnPanelActivate(this);
        if (Activated != null) Activated();
    }
    void ActivateGameObject(){gameObject.SetActive(true);}

    public void Deactivate()
    {
        Deactivate(true);
    }

    public void Deactivate(bool animate)
    {
        Init();

        if (!_isActive) return;
        _isActive = false;

        //Debug.Log("Panel Deactivated: " + name);

        if (animate && Application.isPlaying && gameObject.activeInHierarchy && closeAnim != null)
        {
            if (openAnim != null)
                openAnim.StopAnim();

            closeAnim.StartAnim();
        }
        else DeactivateGameObject();

        if (group != null) group.OnPanelDeactivate(this);
        if (Deactivated != null) Deactivated();
    }
    void DeactivateGameObject(){if(!isActive) gameObject.SetActive(false);}


    public void AddValidation(System.Func<bool> validation)
    {
        validations.Add(validation);
    }

    public void RemoveValidation(System.Func<bool> validation)
    {
        if (validations.Count == 0) return;
        if (!validations.Contains(validation)) return;
        validations.Remove(validation);
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(Panel))]
public class PanelEditor : Editor
{
    Panel script;
    void OnEnable()
    {
        script = target as Panel; 
//        script.isActive = script.gameObject.activeSelf;
        Registger();
    }

    public override void OnInspectorGUI()
    {
        PanelGroup prevGrp = script.group;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Activate"))
        {
            script.Activate(); 
        }
        if (GUILayout.Button("Deactivate"))
        {
            script.Deactivate(); 
        }
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            if (script.group == null && prevGrp != null) prevGrp.UnregisterPanel(script);
            Registger();
        }
    }

    void Registger()
    {
        if(script.group != null) script.group.RegisterPanel(script); 
    }
}
#endif
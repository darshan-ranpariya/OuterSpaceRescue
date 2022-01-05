using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
#endif
public abstract class ComponentTestBase<T> : MonoBehaviour where T : Component
{ 
    protected T target;
    public FunctionButton runTest;
    public bool preventInEditMode = true; 
    public bool runTestOnEnable;

    public abstract void Init();
#if UNITY_EDITOR
    void Awake()
    {
        target = GetComponent<T>();
        runTest = new FunctionButton() { fName = "RunTest", comp = this };
    }
    void OnEnable ()
    { 
        if (preventInEditMode && !Application.isPlaying) return;
        if (target != null) Init();
        if (runTestOnEnable) Test(); 
    }
#endif

    public abstract void Test(); 
    void RunTest()
    {
        if (!(Application.isEditor && !Application.isPlaying && preventInEditMode))
        {
            Test();
        }
    }
}

[Serializable]
public class FunctionButton
{
    public string fName;
    public Component comp; 
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FunctionButton))]
public class FunctionButtonDrawer : PropertyDrawer
{
    FunctionButton target;
    Component targetObject;  
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (target == null)
        {
            target = fieldInfo.GetValue(property.serializedObject.targetObject) as FunctionButton;
            if (target != null) targetObject = target.comp; 
        }


        EditorGUI.BeginProperty(position, label, property);
        bool guiEnabled = GUI.enabled;
        GUI.enabled = (targetObject != null); 

        if (GUI.Button(position, target.fName))
        {
            targetObject.SendMessage(target.fName);
        }

        GUI.enabled = guiEnabled; 
        EditorGUI.EndProperty();
    }
}
#endif 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class ObservableProcessObject : MonoBehaviour
{
    public MonoBehaviour targetClassOcject;
    public string processName;
    public ObservableProcess process;

    void OnEnable()
    {
        Init();
        UpdateObjectState();
    }

    void Init()
    {
        if (targetClassOcject!=null)
        {
            List<string> processes = new List<string>();

            PropertyInfo processFieldInfo = targetClassOcject.GetType().GetProperty(processName);
            if (processFieldInfo != null)
            {
                print(processFieldInfo.GetValue(targetClassOcject, null));
                process = (ObservableProcess)processFieldInfo.GetValue(targetClassOcject, null);
            }
        }
    }

    void UpdateObjectState()
    {

    }
}

#if UNITY_EDITOR 
[CustomEditor(typeof(ObservableProcessObject))]
public class ObservableProcessObjectEditor : Editor
{
    ObservableProcessObject script; 
    string[] processNames = new string[0];
    int selectedOption = 0;

    void OnEnable()
    {
        script = target as ObservableProcessObject;

        if (script.targetClassOcject != null)
        {
            List<string> processes = new List<string>();
            FieldInfo[] myFieldInfo;
            myFieldInfo = script.targetClassOcject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            Type processType = typeof(ObservableProcess);
            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                if (myFieldInfo[i].FieldType.Equals(processType))
                {
                    processes.Add(myFieldInfo[i].Name);
                }
            }
            processNames = processes.ToArray();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        for (int i = 0; i < processNames.Length; i++)
        {
            if (processNames[i].Equals(script.processName))
            {
                selectedOption = i;
                break;
            }
        }
        selectedOption = EditorGUILayout.Popup("Select Process", selectedOption, processNames);
        if (GUI.changed)
        {
            script.processName = processNames[selectedOption];
        }
    }
}
#endif
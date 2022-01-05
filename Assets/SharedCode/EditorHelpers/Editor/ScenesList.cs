using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScenesList : EditorWindow
{
    Vector2 scrollPos;
    string path { get { return Application.dataPath + "/4_Scenes"; } }
    string[] scenesPath;
    string[] scenesName;
    [MenuItem("Window/Scenes")]
    static void Init()
    {
        ScenesList window = (ScenesList)EditorWindow.GetWindow(typeof(ScenesList));
        window.Show();
    }

    void OnEnable()
    {
        titleContent = new GUIContent("Scenes");
        scenesPath = System.IO.Directory.GetFiles(path, "*.Unity");
        if (scenesPath == null) scenesPath = new string[0];
        scenesName = new string[scenesPath.Length];
        for (int i = 0; i < scenesPath.Length; i++)
        {
            string[] splits = scenesPath[i].Split('\\');
            scenesName[i] = splits[splits.Length - 1].Split('.')[0];
        }
    }

    // This will only get called 10 times per second.
    public void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("PC");
        if (scenesPath!=null)
        {
            for (int i = 0; i < scenesPath.Length; i++)
            { 
                if (GUILayout.Button(scenesName[i]))
                {
                    Application.OpenURL(scenesPath[i]);
                }
            }
        }
        EditorGUILayout.EndScrollView();
    } 
}

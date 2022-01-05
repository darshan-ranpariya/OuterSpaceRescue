using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
 
public class GameColors
{
    public class PC
    {
        public static Color red = FromHex("#ff0000");
        public static Color blue = FromHex("#00cafe");
        public static Color darkBlue = FromHex("#005ebe");
        public static Color yellow = FromHex("#ffc900");
        public static Color orange = FromHex("#ff8400");
        public static Color darkGrey = FromHex("#686767");
        public static Color lightGrey = FromHex("#C5C5C5FF");
    }

    public static Color FromHex(string hexCode)
    {
        Color c = Color.white;
        ColorUtility.TryParseHtmlString(hexCode, out c);
        return c;
    }
}



#if UNITY_EDITOR
public class GameColorsWindow : EditorWindow
{
    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Window/GameColors")]
    static void Init()
    {
        GameColorsWindow window = (GameColorsWindow)EditorWindow.GetWindow(typeof(GameColorsWindow));
        window.Show();
    }

    void OnEnable()
    {
        titleContent = new GUIContent("Game Colors");
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
        EditorGUILayout.ColorField(GameColors.PC.red);
        EditorGUILayout.ColorField(GameColors.PC.blue);
        EditorGUILayout.ColorField(GameColors.PC.darkBlue);
        EditorGUILayout.ColorField(GameColors.PC.yellow);
        EditorGUILayout.ColorField(GameColors.PC.orange);
        EditorGUILayout.ColorField(GameColors.PC.darkGrey);
        EditorGUILayout.ColorField(GameColors.PC.lightGrey);

        if (GUILayout.Button("Edit"))
        {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(Application.dataPath + "/1_Scripts/Editor/GameColorsWindow.cs", 9);
        }
        EditorGUILayout.EndScrollView();
    }
}

#endif
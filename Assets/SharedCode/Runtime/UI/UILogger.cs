using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UILogger : MonoBehaviour
{
    static UILogger i;
    public Text text;
    static bool init;
    static List<string> logs = new List<string>();

    static void Init()
    {
        if (!init)
        {
            i = FindObjectOfType<UILogger>();
            init = true;
        }
    }

    static bool available { get { Init(); return (i != null && i.text != null); } }

    public static void Log(string logFormat, params object[] args)
    {
        if (!available) return;

        logs.Add(string.Format(logFormat, args));
        if (logs.Count > 20) logs.RemoveAt(0);

        StringBuilder sb = new StringBuilder();
        for (int i = logs.Count - 1; i >= 0; i--)
        {
            sb.AppendLine(logs[i]);
            sb.AppendLine("-----");
        }

        i.text.text = sb.ToString();
    }
}

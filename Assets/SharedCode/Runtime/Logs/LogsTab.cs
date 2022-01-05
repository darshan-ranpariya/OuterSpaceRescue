using UnityEngine; 
using UnityEngine.UI;
using System.Collections.Generic;
using Logs;

public class LogsTab : MonoBehaviour {

    public Text logsText;
    static List<string> recentLogs = new List<string>();
    static bool subscribedStatic;
    static bool activated = true;

    public void Enable() {
        activated = true;
    }

    public void Disable() {
        activated = false;
    }

    public void Toggle() {
        activated = !activated;
    }

    void Awake()
    { 
        if (subscribedStatic) return;
        Bridge.NewLogAdded += Add;
        subscribedStatic = true;
    }

    void OnEnable()
    {
        if (activated) Bridge.NewLogAdded += UpdateScrollText;
        else {
            gameObject.SetActive(false);
        }
    }

    void OnDisable() {
        if (activated) Bridge.NewLogAdded -= UpdateScrollText;
    }

    static void Add(object log, GameObject gameObj, Type type)
    {
        recentLogs.Add((string)log + "\n");
        if (recentLogs.Count > 20) recentLogs.RemoveAt(0);
    }

    void UpdateScrollText(object log, GameObject gameObj, Type type)
    {
        if (logsText!=null)
        {
            logsText.text = "";
            for (int i = 0; i < recentLogs.Count; i++) logsText.text += recentLogs[i]; 
        }
    }


}

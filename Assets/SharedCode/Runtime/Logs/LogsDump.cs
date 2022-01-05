using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LogsDump : MonoBehaviour
{
    string sessionLogsFileLocation = "";
    List<string> unsavedSessionLogs = new List<string>();
    const string separator = "\n--------------\n";

    void Awake()
    {
#if UNITY_EDITOR
        return;
#endif


        System.DateTime dt = System.DateTime.Now;
        string dir = Application.persistentDataPath;
#if UNITY_ANDROID && !UNITY_EDITOR
        dir = Application.persistentDataPath;
#endif
        sessionLogsFileLocation = string.Format("{0}/Logs_{1:0000}{2:00}{3:00}_{4:00}{5:00}{6:00}.txt", dir, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

        DontDestroyOnLoad(gameObject);

        Logs.Bridge.NewLogAdded += OnNewLogAdded;

        if (!File.Exists(sessionLogsFileLocation)) File.Create(sessionLogsFileLocation);
        Debug.Log("Logs Dump Location: " + sessionLogsFileLocation);
    }

    void OnNewLogAdded(string log, GameObject context, Logs.Type type)
    {
        unsavedSessionLogs.Add(log);
        if (unsavedSessionLogs.Count > 20)
        {
            DumpToFile();
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            DumpToFile();
        }
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            DumpToFile();
        }
    }

    //    int t = 0;
    //    void Update()
    //    {
    //        if (Input.GetKeyDown(KeyCode.L))
    //        {
    //            Logs.Add.Info("TestLog"+t.ToString());
    //            t++;
    //        }
    //    } 

    public void DumpToFile()
    {
        if (string.IsNullOrEmpty(sessionLogsFileLocation)) return;
        if (unsavedSessionLogs.Count == 0) return;
        if (!File.Exists(sessionLogsFileLocation)) return;

        try
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader streamReader = new StreamReader(sessionLogsFileLocation))
            {
                sb.Append(streamReader.ReadToEnd());
                streamReader.Close();
                streamReader.Dispose();
            }

            for (int i = 0; i < unsavedSessionLogs.Count; i++)
            {
                sb.Append(unsavedSessionLogs[i]);
                sb.Append(separator);
            }
            unsavedSessionLogs.Clear();

            using (FileStream fileStream = new FileStream(sessionLogsFileLocation, FileMode.Open, FileAccess.ReadWrite))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(sb.ToString());
                streamWriter.Close();
                fileStream.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("LogsDumpFailed: " + e.Message);
        }
    }
}

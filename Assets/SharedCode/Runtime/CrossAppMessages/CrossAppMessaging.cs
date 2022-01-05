using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CrossAppMessageData
{
    public string to;
    public string subject;
    public string message;

    public override string ToString()
    {
        return string.Format("rec:{0}\n sub:{1}\n msg:{2}", to, subject, message);
    }
}

public class CrossAppMessaging : MonoBehaviour
{
    static string _msgFileFolder;
    static string msgFileFolder
    {
        get
        {
            if (_msgFileFolder == null)
            {
                _msgFileFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Cookies) + "/TeamPokers/";
                //Debug.Log(_msgFileFolder);
            }
            return _msgFileFolder;
        }
    }
    static string msgFileName = "crap.md";
    static string _msgFilePath;
    static string msgFilePath
    {
        get
        {
            if (_msgFilePath == null)
            {
                _msgFilePath = msgFileFolder + msgFileName; 
            }
            return _msgFilePath;
        }
    }

    public static event Action<CrossAppMessageData> MsgReceived;
    public string receiveName;

    void Update()
    {
        if (File.Exists(msgFilePath))
        {
            CrossAppMessageData data = LocalData.Load<CrossAppMessageData>(msgFileName, msgFileFolder);
            if (data!=null && data.to.Equals(receiveName))
            {
                File.Delete(msgFilePath);
                if (MsgReceived != null) MsgReceived(data);
                Logs.Add.Info("CrossAppMsg : " + data.ToString());
                //Toast.Show(data.ToString());
            }
        }
    }

    public static void Send(string _to, string _subject, string _message)
    {
        CrossAppMessageData data = new CrossAppMessageData()
        {
            to = _to,
            subject = _subject,
            message = _message
        };
        try
        {
            LocalData.Save(msgFileName, data, true, msgFileFolder);
        }
        catch { }
    }
}
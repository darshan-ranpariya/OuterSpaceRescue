using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossAppSceneRequests : MonoBehaviour
{   
    public static string subject = "scene"; 

    void OnEnable()
    {
        CrossAppMessaging.MsgReceived += CrossAppMessaging_MsgReceived; 
    }

    void OnDisable()
    {
        CrossAppMessaging.MsgReceived -= CrossAppMessaging_MsgReceived;
    } 

    void CrossAppMessaging_MsgReceived(CrossAppMessageData data)
    {
        if (data.subject.Equals(subject))
        {
            WindowsUtility.BringToForeground();
            try
            {
                if(!SceneManager.GetActiveScene().name.Equals(data.message)) SceneManager.LoadScene(data.message);
            }
            catch
            {
                Toast.Show("thefuckisthisscene " + data.message);
            }
        }
    }

    public static void Request(string sceneName)
    {
        Request("lobby", sceneName);
    }

    public static void Request(string to, string sceneName)
    {
        CrossAppMessaging.Send(to, subject, sceneName);
    }
}

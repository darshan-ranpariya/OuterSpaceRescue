using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class CrossAppPanelRequests : MonoBehaviour
{ 
    [Serializable]
    public class PanelData
    {
        public string key;
        public Panel[] panels;
    }

    public static string subject = "panel";
    public PanelData[] panels = new PanelData[0];

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
            for (int i = 0; i < panels.Length; i++)
            {
                if (data.message.Equals(panels[i].key))
                {
                    for (int p = 0; p < panels[i].panels.Length; p++)
                    {
                        if (!Application.isFocused)
                        {
                            WindowsUtility.BringToForeground();
                        }
                        panels[i].panels[p].Activate();
                    }
                    break;
                }
            }
        }    
    }

    public void Request(string panelName)
    {
        Request("lobby", panelName);
    }

    public void Request(string to, string panelName)
    {
        CrossAppMessaging.Send(to, subject, panelName);
    } 
}

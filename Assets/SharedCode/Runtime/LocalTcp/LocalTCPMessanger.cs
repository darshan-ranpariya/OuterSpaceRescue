using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTCPMessanger : MonoBehaviour
{ 
    public static LocalTCPMessanger _instance;
    public static LocalTCPMessanger instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LocalTCPMessanger>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    [Space]
    public LocalTCPServer[] localTCPServers;
    public LocalTCPClient localTCPClient;

    public static event Action<string> Received;
    public static void Send(string message)
    {
        if (instance != null)
        {
            instance.Send_(message);
        }
    }

    void OnEnable()
    { 
        if (localTCPClient != null) localTCPClient.MessageReceived += LocalTCPServer_MessageReceived;
    }

    void OnDisable()
    { 
        if (localTCPClient != null) localTCPClient.MessageReceived -= LocalTCPServer_MessageReceived;
    }

    private void LocalTCPServer_MessageReceived(string obj)
    {
        if (Received != null) Received(obj);
    }

    public void Send_(string message)
    { 
        if (localTCPServers != null)
        {
            for (int i = 0; i < localTCPServers.Length; i++)
            {
                if (localTCPServers[i] != null) localTCPServers[i].Send(message);
            }
        }
    }
}

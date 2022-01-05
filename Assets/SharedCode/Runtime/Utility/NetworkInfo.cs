using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using SimpleJSON;

public class NetworkInfo : MonoBehaviour
{
    static bool init;

    public static ObservableVariable<string> ipAddress = new ObservableVariable<string>();
    public static ObservableVariable<string> city = new ObservableVariable<string>(); 

    void Awake()
    {
        if (!init)
        {
            init = true;
#if !UNITY_WEBGL
            //ipAddress.Value = Network.player.ipAddress;
#endif
            city.Value = string.Empty;
            new RestAPI.Get("http://ip-api.com/json", OnGetApi);
        }
    }

    static void OnGetApi(string json)
    {
        JSONNode rootNode = JSONNode.Parse(json);

        try { ipAddress.Value = rootNode["query"]; }
        catch
        {
#if !UNITY_WEBGL
            //ipAddress.Value = Network.player.ipAddress;
#endif
        }

        try { city.Value = rootNode["city"]; }
        catch { city.Value = string.Empty; } 

        print(ipAddress.Value);
        print(city.Value); 
    }

}

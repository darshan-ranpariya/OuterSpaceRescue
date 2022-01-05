using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;


public class MonoRestCall : MonoBehaviour
{
    public string url;
    string[] keys;
    string[] vals;

    void OnEnable()
    {
        string urlAddress = "https://dhlottery.co.kr/gameInfo.do?method=powerWinNoList";

        Stopwatch sw = Stopwatch.StartNew();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = null;

            if (response.CharacterSet == null)
            {
                readStream = new StreamReader(receiveStream);
            }
            else
            {
                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
            }

            string data = readStream.ReadToEnd();
            UnityEngine.Debug.Log(TimeSpan.FromTicks(sw.ElapsedTicks).TotalSeconds);
            sw.Stop();
            UnityEngine.Debug.Log(data); 

            response.Close();
            readStream.Close();
        }
        //StartCoroutine("CallAPI");
    }

    IEnumerator CallAPI()
    {
        WWW w = new WWW(url); 
        Stopwatch sw = Stopwatch.StartNew();
        yield return w;
        UnityEngine.Debug.LogFormat("MonoRestCall {0}. Seconds took {1}", name, TimeSpan.FromTicks(sw.ElapsedTicks).TotalSeconds);
        UnityEngine.Debug.LogFormat("MonoRestCall {0}. Result {1}", name, w.text);
        res r = JsonUtility.FromJson<res>(w.text);
        DateTime realLocalTime = DateTime.FromFileTime(long.Parse(r.currentFileTime)).Add(TimeSpan.FromTicks(sw.ElapsedTicks));
        UnityEngine.Debug.Log(realLocalTime.ToString());
        DateTimeExt.systemOffsetSpan = DateTime.Now.Subtract(realLocalTime);
        UnityEngine.Debug.Log(DateTimeExt.systemOffsetSpan.TotalSeconds);
    }

    class res
    {
        public string currentFileTime;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections.Generic;

public class UITimeTicker : MonoBehaviour 
{  
    DateTime refTime;
    [SerializeField]
    string refTimeString;

    public Text text;
    public TMPro.TextMeshProUGUI textPro;
    public bool countdown;
    public bool showHours = true;
    public bool showMinutes = true;
    public bool showSeconds = true;  
    public string format = "00";
    public float updateInterval = 0.25f;
    public TimeSpan diff = default(TimeSpan);

    public string testRefTime;
    public void Test()
    {
//        StartClock(); 
        StartClock(DateTime.Parse(testRefTime)); 
    }

    void Awake()
    {
        //workaround for serialization of daetTime
        if (!string.IsNullOrEmpty(refTimeString)) refTime = DateTime.Parse(refTimeString);
    }

    void OnEnable()
    { 
        StartCoroutine("Clock_c");
    }

    public void StartClock()
    { 
        StartClock(default(DateTime), false, showHours, showMinutes, showSeconds);
    }

    public void StartClock(DateTime _refTime)
    {
        refTimeString = _refTime.ToString();
        StartClock(_refTime, countdown, showHours, showMinutes, showSeconds);
    }

    public void StartClock(DateTime _refTime, bool _countdown, bool _showHours = true, bool _showMinutes = true, bool _showSeconds = true)
    {
        refTime = _refTime;
        countdown = _countdown;
        showHours = _showHours;
        showMinutes = _showMinutes;
        showSeconds = _showSeconds;

        if (gameObject.activeInHierarchy)
        {
            StopCoroutine("Clock_c");
            StartCoroutine("Clock_c");
        }
    }
 
    IEnumerator Clock_c()
    {        
        List<double> ttt = new List<double>();
        bool defaultTime = refTime.Year == 1;

        while (true)
        {
            StringBuilder sb = new StringBuilder();
            ttt.Clear();  
            if(countdown) diff = refTime.Subtract(DateTime.Now);    
            else diff = DateTime.Now.Subtract(refTime);        

            if (showHours)
            {
                if(defaultTime) ttt.Add(diff.Hours); 
                else ttt.Add(diff.TotalHours); 
            }
            if (showMinutes)
            { 
                if(defaultTime || showHours) ttt.Add(diff.Minutes); 
                else ttt.Add(diff.TotalMinutes); 
            }
            if (showSeconds)
            {
                ttt.Add(diff.Seconds);  
            } 

            for (int i = 0; i < ttt.Count; i++)
            {
                ttt[i] = Math.Floor(ttt[i]); 
                if (ttt[i] < 0)
                {
                    sb = new StringBuilder();
                    for (int j = 0; j < ttt.Count; j++) {
                        if (sb.Length>0) sb.Append(":");
                        sb.Append("--");
                    }
                    break;
                }
                if (sb.Length>0) sb.Append(":");
                sb.Append(ttt[i].ToString(format));
            }

            if(text) text.text = sb.ToString(); 
            if(textPro) textPro.text = sb.ToString();
            yield return new WaitForSeconds(updateInterval); 
        }
    } 

}

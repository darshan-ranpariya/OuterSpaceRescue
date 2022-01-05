using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClockEvent
{
    public DateTime invokeTime;
    public Action action; 

    public ClockEvent(DateTime _invokeTime, Action _action)
    {
        action = _action;
        invokeTime = _invokeTime;
    }
}

public class GameClock : MonoBehaviour 
{ 
    static bool instantiated;
    static List<ClockEvent> allEvents = new List<ClockEvent>(); 
    static List<ClockEvent> hourlyEvents = new List<ClockEvent>(); 
    static int lastUpdatedHour = -1; 

    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine("Clock_c");
        gameObject.name = "GameClock";
        instantiated = true; 
    } 

    void OnApplicationPause(bool paused)
    { 
        if (!paused)
        {
            StopCoroutine("Clock_c");  
            StartCoroutine("Clock_c"); 
        } 
    } 

    void OnApplicationFocus (bool focused)
    {  
        if (focused)
        {
            StopCoroutine("Clock_c");  
            StartCoroutine("Clock_c"); 
        } 
    }


    public static void AddEvent(ClockEvent evt)
    {
        if (!instantiated)
        {
            new GameObject().AddComponent<GameClock>();
        }
        allEvents.Add(evt);
        UpdateHourlyList();
    }

    public static void RemoveEvent(ClockEvent evt)
    {
        if (!instantiated)
        {
            new GameObject().AddComponent<GameClock>();
        }
        if (allEvents.Contains(evt))
        {
            allEvents.Remove(evt);
        }
        UpdateHourlyList();
    }

    public static void UpdateHourlyList()
    {
        hourlyEvents.Clear();
        for (int i = 0; i < allEvents.Count; i++)
        { 
            int m = allEvents[i].invokeTime.Subtract(DateTime.Now).Minutes;
            if (m > 0 && m < 65)
            {
                hourlyEvents.Add(allEvents[i]);
            }
        }
    }

    IEnumerator Clock_c()
    {
//        yield return new WaitForSeconds(60);
        while (true)
        {
            print("CheckedOn on " + DateTime.Now.ToLongTimeString());

            int h = System.DateTime.Now.Hour; 

            if (System.DateTime.Now.Hour != lastUpdatedHour)
            {
                lastUpdatedHour = h;
                UpdateHourlyList();
            }

            for (int i = 0; i < hourlyEvents.Count; i++)
            {
                if (hourlyEvents[i].invokeTime.Minute == System.DateTime.Now.Minute)
                {
                    hourlyEvents[i].action();
                    allEvents.Remove(hourlyEvents[i]);
                }
            }

            yield return new WaitForSeconds(60 - System.DateTime.Now.Second);
        }
    } 
}

